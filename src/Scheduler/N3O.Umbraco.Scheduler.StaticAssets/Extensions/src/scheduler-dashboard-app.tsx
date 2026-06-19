// NOTE: React shell is overhead here (the dashboard merely wraps a Hangfire <iframe>) — kept for
// uniformity per migration decision. There is no React-shaped UI: the component renders a single
// iframe, ported verbatim from the legacy AngularJS view N3O.Umbraco.Scheduler.html.
// Styles are applied via adoptedStyleSheets on the shadow root in the host web component
// (scheduler-dashboard.ts) rather than a <style> JSX node.

// React UI for the Scheduler dashboard. Renders the Hangfire backoffice UI in a full-size iframe.
// No props, no state, no backend calls of its own.
export function SchedulerDashboardApp() {
    return (
        <iframe
            name="hangfireIFrame"
            id="hangfire"
            title="Scheduler"
            frameBorder="0"
            scrolling="yes"
            src="/umbraco/backoffice/hangfire/"
            allowFullScreen
        />
    );
}
