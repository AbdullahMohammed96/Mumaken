using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Entities.UserTables;
using Mumaken.Persistence;
using Mumaken.RentalCompanyDashboard;
using Mumaken.RentalCompanyDashboard.PreventBackMiddelWare;
using Mumaken.Service;
using Mumaken.Service.Api.Contract.Auth;
using Mumaken.Service.Api.Contract.Copon;
using Mumaken.Service.Api.Contract.Logic;
using Mumaken.Service.Api.Contract.More;
using Mumaken.Service.Api.Implementation.Auth;
using Mumaken.Service.Api.Implementation.Copon;
using Mumaken.Service.Api.Implementation.Logic;
using Mumaken.Service.Api.Implementation.More;
using Mumaken.Service.DashBoard.Contract.CarInterfaces;
using Mumaken.Service.DashBoard.Contract.NotificationInterfaces;
using Mumaken.Service.DashBoard.Implementation.CarImplementation;
using Mumaken.Service.DashBoard.Implementation.NotificationImplementation;
using NToastNotify;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentity<ApplicationDbUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();
//builder.Services.AddDefaultIdentity<ApplicationDbUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(300); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(300); // Set cookie expiration time
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/Login";
    options.SlidingExpiration = true;
});
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(ModelBindingMessages));
    });

builder.Services.AddControllersWithViews()
    .AddNToastNotifyToastr(new ToastrOptions()
    {
        ProgressBar = true,
        PositionClass = ToastPositions.TopRight,
        PreventDuplicates = true,
        CloseButton = true
    });
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("ar-EG"),
        new CultureInfo("en-US")
    };

    options.DefaultRequestCulture = new RequestCulture(supportedCultures[0], supportedCultures[0]);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.RequestCultureProviders = new List<IRequestCultureProvider>()
        {
            // Order is important, its in which order they will be evaluated
            new QueryStringRequestCultureProvider(),
            new CookieRequestCultureProvider(),
        };
});
builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();
builder.Services.AddTransient<INotificationServices, NotificationServices>();
builder.Services.AddTransient<IMoreService, MoreService>();

builder.Services.AddTransient<IUserServices, UserServices>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddScoped<IHelper, Helper>();
builder.Services.AddScoped<ICarServices, CarServices>();
builder.Services.AddScoped<IOrderProvider, OrderProvider>();
builder.Services.AddScoped<IOrderClient, OrderClient>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddSignalR();
var app = builder.Build();

app.UseSession();
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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<PreventBackToLoginMiddleware>();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Orders}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Orders}/{action=Index}/{id?}");

    // add endpoints to chatHub
    endpoints.MapHub<ChatHub>("/chatHub");
});

app.MapRazorPages();

app.Run();
