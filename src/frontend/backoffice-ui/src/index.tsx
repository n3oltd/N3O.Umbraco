import { useCallback, useEffect, useRef, useState } from 'react';

// Thin React wrappers over the backoffice's UUI web components, so apps get idiomatic, controlled React
// components instead of re-implementing the controls (and their styling) by hand. They handle two
// non-obvious things: (1) UUI exposes state as DOM *properties* (el.checked / el.value / el.options) and
// emits *custom* events, neither of which React's attribute/synthetic-event model binds for custom
// elements — so props and events are bridged through the element (useProperty / useEvent); and (2) the
// form-associated UUI controls cannot be created by React at all and must be parser-built — see
// useUuiControl for the full explanation. Consumers see none of this; every wrapper is just plain props.

// Create a UUI control from an HTML string via the parser instead of letting React create it.
//
// WHY THIS EXISTS (non-obvious): the *form-associated* UUI controls — uui-select, uui-checkbox, uui-toggle,
// uui-radio-group, uui-button — set an attribute in their constructor (UUIFormControlMixin sets `pristine`).
// Per the Custom Elements spec that is legal when the browser's HTML parser upgrades the element (the path
// Lit's html`` templates and the native backoffice use) but ILLEGAL for synchronous `document.createElement`,
// which is exactly how React renders host elements. So rendering `<uui-select/>` in JSX throws
// `NotSupportedError: The result must not have attributes`, the element never upgrades, and it renders as an
// invisible 0-height inert tag. Non-form-associated UUI elements (box, icon, loader-bar, radio) are fine in
// JSX — only this subset needs the parser path.
//
// We build the element from a parsed fragment into a `display:contents` host (which adds no layout box of
// its own) and expose it as STATE: when `html` changes (e.g. a dynamic option set), React's ref-callback
// rule rebuilds the element, and surfacing it as state means the useProperty/useEvent effects re-run and
// re-sync the new element. `html` is value-compared in the dep array, so a stable markup string never
// re-creates the element. Consumers see none of this — the wrappers' props are unchanged.
function useUuiControl(html: string): [(host: HTMLElement | null) => (() => void) | void, HTMLElement | null] {
    const [element, setElement] = useState<HTMLElement | null>(null);

    const mount = useCallback((host: HTMLElement | null): (() => void) | void => {
        if (!host) {
            return;
        }

        const created = document.createRange().createContextualFragment(html).firstElementChild as HTMLElement;
        host.appendChild(created);
        setElement(created);

        return () => {
            created.remove();
            setElement(null);
        };
    }, [html]);

    return [mount, element];
}

// Assign a DOM property whenever the value changes (UUI reads these, not the matching attribute). Re-runs
// against a freshly-built element too, since `element` is a dependency.
function useProperty(element: HTMLElement | null, name: string, value: unknown): void {
    useEffect(() => {
        if (element) {
            (element as unknown as Record<string, unknown>)[name] = value;
        }
    }, [element, name, value]);
}

// Subscribe to a DOM event on the control (UUI's `change`, or `click` for buttons). A ref holds the latest
// handler so a new closure each render doesn't tear down and re-add the listener; the subscription re-binds
// if the element is rebuilt.
function useEvent(element: HTMLElement | null, type: string, handler: ((event: Event) => void) | undefined): void {
    const saved = useRef(handler);
    saved.current = handler;

    useEffect(() => {
        if (!element) {
            return;
        }

        const listener = (event: Event): void => saved.current?.(event);
        element.addEventListener(type, listener);

        return () => element.removeEventListener(type, listener);
    }, [element, type]);
}

// Escape a string for safe interpolation into a double-quoted HTML attribute in useUuiControl markup.
function attr(value: string): string {
    return value.replace(/&/g, '&amp;').replace(/"/g, '&quot;').replace(/</g, '&lt;');
}

type UuiLook = 'default' | 'primary' | 'secondary' | 'outline' | 'placeholder';
type UuiColor = 'default' | 'positive' | 'warning' | 'danger';

interface UuiButtonProps {
    label: string;
    look?: UuiLook;
    color?: UuiColor;
    disabled?: boolean;
    compact?: boolean;
    href?: string;
    icon?: string;
    onClick?: () => void;
}

// uui-button is form-associated too (it can be a submit/reset button), so it is parser-built like the other
// form controls. An optional leading `icon` is part of the parsed markup (use it instead of slotting a
// uui-icon child); label/look/color/href/disabled are driven as properties, and `click` is bound as an event.
export function UuiButton({ label, look = 'default', color = 'default', disabled, compact, href, icon, onClick }: UuiButtonProps) {
    const inner = icon ? `<uui-icon name="${attr(icon)}"></uui-icon> ${attr(label)}` : '';
    const [mount, element] = useUuiControl(`<uui-button>${inner}</uui-button>`);

    useProperty(element, 'label', label);
    useProperty(element, 'look', look);
    useProperty(element, 'color', color);
    useProperty(element, 'compact', !!compact);
    useProperty(element, 'href', href || undefined);
    useProperty(element, 'disabled', !!disabled);
    useEvent(element, 'click', onClick);

    return <span style={{ display: 'contents' }} ref={mount}></span>;
}

interface UuiSelectOption {
    name: string;
    value: string;
}

interface UuiSelectProps {
    options: UuiSelectOption[];
    value: string;
    placeholder?: string;
    disabled?: boolean;
    onChange: (value: string) => void;
}

export function UuiSelect({ options, value, placeholder, disabled, onChange }: UuiSelectProps) {
    const [mount, element] = useUuiControl('<uui-select></uui-select>');

    useProperty(element, 'options', options.map((option) => ({ ...option, selected: option.value === value })));
    useProperty(element, 'placeholder', placeholder ?? '');
    useProperty(element, 'disabled', !!disabled);
    useEvent(element, 'change', () => onChange((element as { value?: string } | null)?.value ?? ''));

    return <span style={{ display: 'contents' }} ref={mount}></span>;
}

interface UuiBooleanProps {
    label: string;
    checked: boolean;
    disabled?: boolean;
    onChange: (checked: boolean) => void;
}

export function UuiToggle({ label, checked, disabled, onChange }: UuiBooleanProps) {
    const [mount, element] = useUuiControl('<uui-toggle></uui-toggle>');

    useProperty(element, 'label', label);
    useProperty(element, 'checked', checked);
    useProperty(element, 'disabled', !!disabled);
    useEvent(element, 'change', () => onChange(!!(element as { checked?: boolean } | null)?.checked));

    return <span style={{ display: 'contents' }} ref={mount}></span>;
}

export function UuiCheckbox({ label, checked, disabled, onChange }: UuiBooleanProps) {
    const [mount, element] = useUuiControl('<uui-checkbox></uui-checkbox>');

    useProperty(element, 'label', label);
    useProperty(element, 'checked', checked);
    useProperty(element, 'disabled', !!disabled);
    useEvent(element, 'change', () => onChange(!!(element as { checked?: boolean } | null)?.checked));

    return <span style={{ display: 'contents' }} ref={mount}></span>;
}

interface UuiRadioOption {
    label: string;
    value: string;
}

interface UuiRadioGroupProps {
    name: string;
    value: string;
    options: UuiRadioOption[];
    disabled?: boolean;
    onChange: (value: string) => void;
}

// uui-radio-group is form-associated, so it must be parser-built (see useUuiControl). Building the whole
// group + its radios as one markup string means the radios are real parsed children the group adopts on
// upgrade. The selected radio's `checked` is set directly (not just the group `value`) because the group
// only applies `value` to radios it has already collected via the async `slotchange`, so a post-mount
// `value` push can otherwise land before the radios are registered and select nothing.
export function UuiRadioGroup({ name, value, options, disabled, onChange }: UuiRadioGroupProps) {
    const radios = options
        .map((option) => `<uui-radio value="${attr(option.value)}" label="${attr(option.label)}"></uui-radio>`)
        .join('');
    const [mount, element] = useUuiControl(`<uui-radio-group name="${attr(name)}">${radios}</uui-radio-group>`);

    useEffect(() => {
        if (!element) {
            return;
        }

        element.querySelectorAll('uui-radio').forEach((radio) => {
            (radio as unknown as { checked: boolean }).checked = radio.getAttribute('value') === value;
        });
        (element as unknown as { value: string }).value = value;
    }, [element, value]);

    useProperty(element, 'disabled', !!disabled);
    useEvent(element, 'change', () => onChange((element as { value?: string } | null)?.value ?? ''));

    return <span style={{ display: 'contents' }} ref={mount}></span>;
}

interface UuiFileDropzoneProps {
    accept?: string;
    label?: string;
    disabled?: boolean;
    onChange: (files: File[]) => void;
}

// uui-file-dropzone is NOT form-associated (it would survive JSX) — we build it the same parser way only for
// a uniform host pattern. It only drag-drops by default: no built-in click-to-browse and no `disabled`, so we
// open its native picker via `browse()` on host click, and gate BOTH interactions (click and drop) on the
// host when disabled. Selected files arrive on the `change` event's `detail.files` (a File[]), not a property.
export function UuiFileDropzone({ accept, label, disabled, onChange }: UuiFileDropzoneProps) {
    const [mount, element] = useUuiControl('<uui-file-dropzone></uui-file-dropzone>');

    useProperty(element, 'accept', accept ?? '');
    useProperty(element, 'label', label ?? '');
    useEvent(element, 'change', (event) => {
        if (disabled) {
            return;
        }
        onChange((event as CustomEvent<{ files: File[] }>).detail?.files ?? []);
    });

    const browse = (): void => {
        if (!disabled) {
            (element as { browse?: () => void } | null)?.browse?.();
        }
    };

    return (
        <div
            onClick={browse}
            style={disabled ? { pointerEvents: 'none', opacity: 0.5 } : { cursor: 'pointer' }}
            ref={mount}></div>
    );
}
