using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.DTO.AppClientDTO.ContactUsClientDto;
using Mumaken.Domain.DTO.AuthApiDTO;
using Mumaken.Domain.DTO.AuthDTO;
using Mumaken.Domain.Entities.UserTables;
using Mumaken.Persistence;
using Mumaken.Service.Api.Contract.Auth;
using Mumaken.Service.Api.Contract.Chat;
using Mumaken.Service.Api.Contract.Copon;
using Mumaken.Service.Api.Contract.Logic;
using Mumaken.Service.Api.Implementation.Auth;
using Mumaken.Service.Api.Implementation.Chat;
using Mumaken.Service.Api.Implementation.Copon;
using Mumaken.Service.Api.Implementation.Logic;
using Mumaken.Service.DashBoard.Contract.AdminInterfaces;
using Mumaken.Service.DashBoard.Contract.ContactUsInterfaces;
using Mumaken.Service.DashBoard.Contract.CoponsInterfaces;
using Mumaken.Service.DashBoard.Contract.HomeInterfaces;
using Mumaken.Service.DashBoard.Contract.NotificationInterfaces;
using Mumaken.Service.DashBoard.Contract.SettingServices;
using Mumaken.Service.DashBoard.Implementation.AdminImplementation;
using Mumaken.Service.DashBoard.Implementation.ContactUsImplementation;
using Mumaken.Service.DashBoard.Implementation.CoponsImplementation;
using Mumaken.Service.DashBoard.Implementation.HomeImplementation;
using Mumaken.Service.DashBoard.Implementation.NotificationImplementation;
using Mumaken.Service.DashBoard.Implementation.SettingServices;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using Mumaken.Service.Api.Contract.More;
using Mumaken.Service.Api.Implementation.More;
using Mumaken.Service.Api.Contract.General;
using Mumaken.Service.Api.Implementation.General;
using Mumaken.Service.DashBoard.Contract.LocationInterfaces;
using Mumaken.Service.DashBoard.Implementation.LocationImplementation;
using Mumaken.Service.DashBoard.Contract.CarInterfaces;
using Mumaken.Service.DashBoard.Implementation.CarImplementation;

namespace Mumaken.Infrastructure.Extension
{
    public static class ConfigureServiceContainer
    {
        public static void AddDbContextServices(this IServiceCollection services,
             IConfiguration Configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection")));


        }

        public static void AddSingletonServices(this IServiceCollection services)
        {


        }

        public static void AddLocalizationServices(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("ar"),
                    new CultureInfo("en")
                };

                options.DefaultRequestCulture = new RequestCulture(supportedCultures[1]);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

        }

        public static void TimeOutServices(this IServiceCollection services, IWebHostEnvironment Environment)
        {
            services.AddDataProtection()
                              .SetApplicationName($"my-app-{Environment.EnvironmentName}")
                              .PersistKeysToFileSystem(new DirectoryInfo($@"{Environment.ContentRootPath}\keys"));

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(300);
            });


        }

        public static void AddDefaultIdentityServices(this IServiceCollection services)
        {

            services.AddDefaultIdentity<ApplicationDbUser>(options =>
            {
                // Default Password settings.
                options.User.RequireUniqueEmail = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            }).AddRoles<IdentityRole>().AddDefaultUI().AddEntityFrameworkStores<ApplicationDbContext>();
        }
        public static void AddJwtServices(this IServiceCollection services, IConfiguration Configuration)
        {

            services.AddAuthentication(options =>
            {
                //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                {

                    options.SaveToken = true;
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = Configuration["Jwt:Site"],
                        ValidIssuer = Configuration["Jwt:Site"],

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SigningKey"]))
                    };
                });

        }
        public static void AddScopedServices(this IServiceCollection services)
        {
            services.AddScoped<IHelper, Helper>();
            services.AddScoped<IChatService, ChatService>();
        }

        public static void AddTransientServices(this IServiceCollection services)
        {
            #region FluentValidation
            services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();
            services.AddTransient<IValidator<UserAddDTO>, RegisterDtoValidator>();

            services.AddValidatorsFromAssemblyContaining<ConfirmCodeDtoValidator>();
            services.AddTransient<IValidator<ConfirmCodeAddDto>, ConfirmCodeDtoValidator>();

            services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();
            services.AddTransient<IValidator<LoginDto>, LoginDtoValidator>();

            services.AddValidatorsFromAssemblyContaining<ChangePasswordDtoValidator>();
            services.AddTransient<IValidator<ChangePasswordDto>, ChangePasswordDtoValidator>();

            services.AddValidatorsFromAssemblyContaining<ForgetPasswordDtoValidator>();
            services.AddTransient<IValidator<ForgetPasswordAddDto>, ForgetPasswordDtoValidator>();

            services.AddValidatorsFromAssemblyContaining<ChangePasswordByCodeDtoValidator>();
            services.AddTransient<IValidator<ChangePasswordByCodeDto>, ChangePasswordByCodeDtoValidator>();

            services.AddValidatorsFromAssemblyContaining<ContactUsDtoValidator>();
            services.AddTransient<IValidator<ContactUsClientAddDto>, ContactUsDtoValidator>();


            #endregion

            services.AddTransient<IOrderClient, OrderClient>();
            services.AddTransient<IOrderProvider, OrderProvider>();
          
          
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IHelper, Helper>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ICouponService, CouponService>();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigration>();
            services.AddTransient<IConfigureOptions<SwaggerUIOptions>, SwaggerUIConfiguration>();
            services.AddSwaggerGen();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<ILanguageManager, LanguageManager>();
            services.AddTransient<IAppService, AppService>();
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<IMoreService, MoreService>();
            services.AddTransient<IGeneralService, GeneralService>();


            #region DashBoard
            services.AddTransient<ISettingServices, SettingServices>();
            services.AddTransient<IContactUsServices, ContactUsServices>();
            services.AddTransient<ICoponServices, CoponServices>();
            services.AddTransient<IHomeServices, HomeServices>();
            services.AddTransient<INotificationServices, NotificationServices>();
            services.AddTransient<IAdminServices, AdminServices>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<ICarServices, CarServices>();
            #endregion
        }


        public static void AddCorsServices(this IServiceCollection services)
        {
            //services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(
            //        builder =>
            //        {
            //            builder.WithOrigins("https://localhost:44306/")
            //            .AllowAnyHeader()
            //            .AllowAnyMethod()
            //            .AllowCredentials();
            //        });
            //});
        }

        public static void AddSessionServices(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(1);//You can set Time   
            });
        }

        public static void AddController(this IServiceCollection services)
        {
            services.AddControllers();
        }


        public static void AddFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<IStartup>();
        }

    }
}
