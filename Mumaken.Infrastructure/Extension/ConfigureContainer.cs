using Microsoft.AspNetCore.Builder;

namespace Mumaken.Infrastructure.Extension
{
    public static class ConfigureContainer
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            // app.UseMiddleware<CustomExceptionMiddleware>();
        }

        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                // جميع الوان وثيمات للسواجر
                //options.InjectStylesheet("/SwaggerCss3.x/theme-feeling-blue.css");
                //options.InjectStylesheet("/SwaggerCss3.x/theme-flattop.css");
                options.InjectStylesheet("/SwaggerCss3.x/theme-material.css");
                //options.InjectStylesheet("/SwaggerCss3.x/theme-monokai.css");
                //options.InjectStylesheet("/SwaggerCss3.x/theme-muted.css");
                //options.InjectStylesheet("/SwaggerCss3.x/theme-newspaper.css");
                // options.InjectStylesheet("/SwaggerCss3.x/theme-outline.css");
            });

        }
        public static void Configurelocalization(this IApplicationBuilder app)
        {
            #region localization

            //var supportedCultures = new[] {
            //    new CultureInfo("ar"),
            //    new CultureInfo("en"),
            //    new CultureInfo("ur") };
            //supportedCultures[0].DateTimeFormat = supportedCultures[1].DateTimeFormat;
            //supportedCultures[0].NumberFormat = supportedCultures[1].NumberFormat;

            //var localizationOptions = new RequestLocalizationOptions
            //{
            //    DefaultRequestCulture = new RequestCulture(supportedCultures[0]),
            //    SupportedCultures = supportedCultures,
            //    SupportedUICultures = supportedCultures,
            //    RequestCultureProviders = new List<IRequestCultureProvider>()
            //    {
            //        // Order is important, its in which order they will be evaluated
            //        new QueryStringRequestCultureProvider(),
            //        new CookieRequestCultureProvider(),
            //    }
            //};

            //app.UseRequestLocalization(localizationOptions);

            #endregion

        }

    }
}
