import { useEffect, useRef, type ReactNode, type RefObject } from 'react';

// Thin React wrappers over the backoffice's UUI web components. They exist because UUI exposes control
// state as DOM *properties* (el.checked / el.value / el.options) and emits *custom* `change` events, both
// of which React's attribute/synthetic-event model does not bind for custom elements. Each wrapper takes
// plain React props and bridges them to the element via a ref, so apps get idiomatic, controlled React
// components instead of re-implementing the controls (and their styling) by hand.

// Assign a DOM property whenever the React value changes (UUI reads these, not the matching attribute).
function useProperty(ref: RefObject<HTMLElement | null>, name: string, value: unknown): void {
    useEffect(() => {
        if (ref.current) {
            (ref.current as unknown as Record<string, unknown>)[name] = value;
        }
    }, [ref, name, value]);
}

// Subscribe to the element's `change` CustomEvent. A ref holds the latest handler so a new closure each
// render doesn't tear down and re-add the listener.
function useChange(ref: RefObject<HTMLElement | null>, handler: () => void): void {
    const saved = useRef(handler);
    saved.current = handler;

    useEffect(() => {
        const el = ref.current;
        if (!el) {
            return;
        }

        const listener = (): void => saved.current();
        el.addEventListener('change', listener);

        return () => el.removeEventListener('change', listener);
    }, [ref]);
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
    onClick?: () => void;
    children?: ReactNode;
}

// `click` is a native bubbling event, so onClick binds directly; look/color/compact are attributes. With
// no children, uui-button renders `label` as its text; pass children to add an icon alongside it.
export function UuiButton({ label, look = 'default', color = 'default', disabled, compact, href, onClick, children }: UuiButtonProps) {
    return (
        <uui-button
            label={label}
            look={look}
            color={color}
            href={href}
            disabled={disabled || undefined}
            compact={compact || undefined}
            onClick={onClick}>
            {children}
        </uui-button>
    );
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
    const ref = useRef<HTMLElement>(null);

    useProperty(ref, 'options', options.map((option) => ({ ...option, selected: option.value === value })));
    useChange(ref, () => onChange((ref.current as { value?: string } | null)?.value ?? ''));

    return <uui-select ref={ref} placeholder={placeholder} disabled={disabled || undefined}></uui-select>;
}

interface UuiBooleanProps {
    label: string;
    checked: boolean;
    disabled?: boolean;
    onChange: (checked: boolean) => void;
}

export function UuiToggle({ label, checked, disabled, onChange }: UuiBooleanProps) {
    const ref = useRef<HTMLElement>(null);

    useProperty(ref, 'checked', checked);
    useChange(ref, () => onChange(!!(ref.current as { checked?: boolean } | null)?.checked));

    return <uui-toggle ref={ref} label={label} disabled={disabled || undefined}></uui-toggle>;
}

export function UuiCheckbox({ label, checked, disabled, onChange }: UuiBooleanProps) {
    const ref = useRef<HTMLElement>(null);

    useProperty(ref, 'checked', checked);
    useChange(ref, () => onChange(!!(ref.current as { checked?: boolean } | null)?.checked));

    return <uui-checkbox ref={ref} label={label} disabled={disabled || undefined}></uui-checkbox>;
}

interface UuiRadioGroupProps {
    name: string;
    value: string;
    disabled?: boolean;
    onChange: (value: string) => void;
    children?: ReactNode;
}

export function UuiRadioGroup({ name, value, disabled, onChange, children }: UuiRadioGroupProps) {
    const ref = useRef<HTMLElement>(null);

    useProperty(ref, 'value', value);
    useChange(ref, () => onChange((ref.current as { value?: string } | null)?.value ?? ''));

    return (
        <uui-radio-group ref={ref} name={name} disabled={disabled || undefined}>
            {children}
        </uui-radio-group>
    );
}

interface UuiRadioProps {
    label: string;
    value: string;
    disabled?: boolean;
}

export function UuiRadio({ label, value, disabled }: UuiRadioProps) {
    return <uui-radio label={label} value={value} disabled={disabled || undefined}></uui-radio>;
}
