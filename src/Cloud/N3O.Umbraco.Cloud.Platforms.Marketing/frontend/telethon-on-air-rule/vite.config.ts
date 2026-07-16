import { n3oPluginConfig } from '@repo/build-config';

export default n3oPluginConfig({
    entries: {
        'telethon-on-air-rule/segment-rule-telethon-on-air': 'src/segment-rule-telethon-on-air.ts',
    },
    outDir: 'dist',
});
