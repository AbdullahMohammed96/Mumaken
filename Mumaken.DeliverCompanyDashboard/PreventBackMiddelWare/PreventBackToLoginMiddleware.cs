namespace Mumaken.DeliverCompanyDashboard.PreventBackMiddelWare
{
    public class PreventBackToLoginMiddleware
    {
        private readonly RequestDelegate _next;

        public PreventBackToLoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated && context.Request.Path.StartsWithSegments("/Identity/Account/Login", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.Redirect("/Orders/Index");
                return;
            }

            await _next(context);
        }
    }
}
