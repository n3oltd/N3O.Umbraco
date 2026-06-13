// NOTE: React shell is overhead here (a near-static help panel) — kept for uniformity per
// migration decision. Hybrid UI: uui-box provides the backoffice-standard chrome; the body is
// plain static markup ported verbatim from the original AngularJS/Lit dashboard view.

// React UI for the Welcome dashboard. Static help/support panel — no props, no state, no backend.
export function WelcomeDashboardApp() {
    return (
        <uui-box headline="Help & Support">
            <p>
                Please visit the N3O Support Centre to view the latest help articles, documentation and to
                contact our support team with any queries.
            </p>

            <p>
                <a href="https://support.n3o.ltd" target="_blank" rel="noopener">
                    Visit Support Centre &rarr;
                </a>
            </p>

            <style>{styles}</style>
        </uui-box>
    );
}

const styles = `
    :host { display: block; }
    p { margin: 0 0 var(--uui-size-space-4); }
    p:last-of-type { margin-bottom: 0; }
    a { color: var(--uui-color-interactive); }
`;
