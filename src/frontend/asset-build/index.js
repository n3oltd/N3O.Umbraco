import crypto from 'node:crypto';
import fs from 'node:fs';
import path from 'node:path';
import {produceAsset} from './assets.js';

// Written next to the emitted assets and read at runtime by N3O.Umbraco.Bundling's IAssetManifest.
// The tag helpers cannot know a content hash at authoring time; this file is how they learn it.
export async function buildAssets(config) {
    const root = config.root ?? process.cwd();
    const outDir = path.resolve(root, config.outDir);
    const publicPath = trimTrailingSlash(config.publicPath ?? '/assets');
    const manifestPath = path.resolve(root, config.manifest ?? path.join(config.outDir, 'assets-manifest.json'));
    const manifest = {};

    for (const [bundleName, bundle] of Object.entries(config.bundles)) {
        manifest[bundleName] = {
            css: await emitAll(bundle.css ?? [], 'css', bundle, {root, outDir, publicPath, config}),
            js: await emitAll(bundle.js ?? [], 'js', bundle, {root, outDir, publicPath, config})
        };
    }

    fs.mkdirSync(path.dirname(manifestPath), {recursive: true});
    fs.writeFileSync(manifestPath, `${JSON.stringify(manifest, null, 2)}\n`);

    return manifest;
}

async function emitAll(entries, kind, bundle, context) {
    const references = [];

    for (const entry of entries) {
        references.push(await emit(entry, kind, bundle, context));
    }

    return references;
}

async function emit(entry, kind, bundle, context) {
    const {root, outDir, publicPath, config} = context;
    const sourceMaps = (entry.sourcemaps ?? bundle.sourcemaps ?? config.sourcemaps ?? 'none') === 'external';
    const wantsIntegrity = entry.integrity ?? bundle.integrity ?? config.integrity ?? false;
    const targets = entry.targets ?? bundle.targets ?? config.targets;

    const produced = await produceAsset(entry, kind, {root, sourceMaps, targets});

    // The sourceMappingURL comment cannot be hashed with the content that names the file it points at,
    // so the map itself goes into the hash instead. Without it, toggling sourcemaps would leave the
    // filename unchanged while the served bytes changed, and a cached copy would fail its integrity check.
    const emitMap = sourceMaps && produced.map != null;
    const hashInput = emitMap ? Buffer.concat([produced.content, produced.map]) : produced.content;

    const hash = crypto.createHash('sha256').update(hashInput).digest('hex').slice(0, 8);
    const outName = hashName(entry.out, hash);
    const outPath = path.resolve(outDir, outName);

    fs.mkdirSync(path.dirname(outPath), {recursive: true});

    let content = produced.content;
    const url = `${publicPath}/${outName.split(path.sep).join('/')}`;

    if (emitMap) {
        const mapName = `${path.basename(outName)}.map`;

        fs.writeFileSync(`${outPath}.map`, produced.map);

        const comment = kind === 'css' ? `\n/*# sourceMappingURL=${mapName} */\n`
                                       : `\n//# sourceMappingURL=${mapName}\n`;

        content = Buffer.concat([content, Buffer.from(comment)]);
    }

    fs.writeFileSync(outPath, content);

    const reference = {url: url, integrity: null};

    if (wantsIntegrity) {
        reference.integrity = `sha384-${crypto.createHash('sha384').update(content).digest('base64')}`;
    }

    // Rollup's iife output must load as a classic script; loading it as a module would defer it, scope
    // it, and force strict mode. Only an entry that says so is emitted with type="module".
    if (kind === 'js') {
        reference.module = entry.module ?? false;
    }

    return reference;
}

function hashName(out, hash) {
    const dir = path.dirname(out);
    const ext = path.extname(out);
    const stem = path.basename(out, ext);
    const name = `${stem}.${hash}${ext}`;

    return dir === '.' ? name : path.join(dir, name);
}

function trimTrailingSlash(value) {
    return value.endsWith('/') ? value.slice(0, -1) : value;
}
