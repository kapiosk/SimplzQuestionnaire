using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimplzQuestionnaire.Interfaces;
using SimplzQuestionnaire.Services;

namespace SimplzQuestionnaire.CMS
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);
            
            services.AddHttpContextAccessor();

            services.AddAntiforgery(x => x.HeaderName = configuration["Antiforgery"]);
            services.AddRazorPages(opts =>
            {
                opts.Conventions.AuthorizeFolder("/");
                opts.Conventions.AllowAnonymousToPage("/Authorization/Login");
            });

            services.AddSingleton<ICurrentUserService, CurrentUserService>();

            services.AddControllers()
                    .AddJsonOptions(opt =>
                    {
                        opt.JsonSerializerOptions.PropertyNamingPolicy = null;
                        opt.JsonSerializerOptions.AllowTrailingCommas = true;
                        opt.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                    });

            services.Configure<CookiePolicyOptions>(opts =>
            {
                opts.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
                opts.Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
            });

            return services;
        }
    }
}
