using Mumaken.Domain.Entities.UserTables;
using Mumaken.Infrastructure.Extension;
using Mumaken.Persistence;
using Mumaken.Persistence.Seeds;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Mumaken.Service.Mapping;
using NToastNotify;
using Mumaken;
using Mumaken.Service;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);
// Configure services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
                            builder => builder.WithOrigins(
                                "https://admin.mumaken.com",  // First allowed origin    Rental Company Dashboard
                                "https://delivery.mumaken.com"     // Second allowed origin  Delivery company dashboard
                            )
                          //builder => builder.WithOrigins("https://admin.mumaken.com") // Your client URL
                          //builder => builder.WithOrigins("https://delivery.mumaken.com") // Your client URL
                          //builder => builder.WithOrigins("https://localhost:7209") // Your client URL
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials()); // Allow credentials
});

MapsterMapping.ConfigureMappings();
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddController();
builder.Services.AddControllersWithViews()
    .AddNToastNotifyToastr(new ToastrOptions()
    {
        ProgressBar = true,
        PositionClass = ToastPositions.TopRight,
        PreventDuplicates = true,
        CloseButton = true
    });
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

builder.Services.AddDbContextServices(builder.Configuration);
builder.Services.AddDefaultIdentityServices();


builder.Services.AddLocalizationServices();
builder.Services.AddControllersWithViews(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
builder.Services.AddCorsServices();
builder.Services.AddRazorPages();
builder.Services.AddSingletonServices();
builder.Services.AddScopedServices();
builder.Services.AddTransientServices();
builder.Services.AddSession();
builder.Services.AddJwtServices(builder.Configuration);
builder.Services.TimeOutServices(builder.Environment);

builder.Services.AddAutoMapper(typeof(IStartup));
builder.Services.AddSignalR();
//builder.Services.AddAuthorization();

builder.Services.AddFluentValidation();
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "momaken-7e9dc-firebase-adminsdk-qe4mf-41eaa382b4.json")),
});
//builder.Services.AddLocalization();

//builder.Services.AddLocalizationServices();





#region localization

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

#endregion


//builder.Services.Configure<RequestLocalizationOptions>(options =>
//{
//    var supportedCultures = new[] { "ar", "en" };

//    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(supportedCultures[0]);
//    options.SetDefaultCulture(supportedCultures[0])
//        .AddSupportedCultures(supportedCultures)
//     .AddSupportedUICultures(supportedCultures);
//});
//builder.Services.AddMvc()
//    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
//    .AddDataAnnotationsLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("ar"), new CultureInfo("en") };


    options.DefaultRequestCulture = new RequestCulture(supportedCultures[1]);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(ModelBindingMessages));
    });

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

#region Data Protection
builder.Services.AddDataProtection().SetApplicationName($"my-app-{builder.Environment.EnvironmentName}")
.SetDefaultKeyLifetime(TimeSpan.FromDays(365))
.PersistKeysToFileSystem(new DirectoryInfo($@"{builder.Environment.ContentRootPath}\keys"));
#endregion




var app = builder.Build();
// should be after build
app.UseCors("AllowSpecificOrigin"); // Use the CORS policy

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationDbUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var applicationDbContext = services.GetRequiredService<ApplicationDbContext>();
    await ContextSeed.Seed(userManager, roleManager, applicationDbContext);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.ConfigureSwagger();

var supportedCultures = new[] { new CultureInfo("ar"), new CultureInfo("en") };
supportedCultures[0].DateTimeFormat = supportedCultures[1].DateTimeFormat;

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(supportedCultures[1]),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new List<IRequestCultureProvider>()
                {
                    // Order is important, its in which order they will be evaluated
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider(),
                }
};

app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    // add endpoints to chatHub
    endpoints.MapHub<ChatHub>("/chatHub");

    endpoints.MapRazorPages();
});

app.MapRazorPages();

app.Run();
