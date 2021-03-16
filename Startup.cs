using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimplzQuestionnaire.Interfaces;
using SimplzQuestionnaire.Services;

namespace SimplzQuestionnaire
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddHttpContextAccessor();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();

            services.AddDbContext<Model.SQContext>(opts => opts.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers()
                    .AddJsonOptions(opt =>
                    {
                        opt.JsonSerializerOptions.PropertyNamingPolicy = null;
                        opt.JsonSerializerOptions.AllowTrailingCommas = true;
                        opt.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                    });

            services.AddAntiforgery(x => x.HeaderName = Configuration["Antiforgery"]);
            services.AddRazorPages(opts =>
            {
                opts.Conventions.AuthorizeFolder("/");
                opts.Conventions.AllowAnonymousToPage("/Authorization/Login");
            });

            services.AddScoped<SignInManager<Model.QuestionnaireUser>>();

            services.AddIdentityCore<Model.QuestionnaireUser>(opts => Configuration.Bind("IdentityCoreSettings", opts))
                    .AddEntityFrameworkStores<Model.SQContext>();

            services.AddAuthentication(IdentityConstants.ApplicationScheme)
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts => Configuration.Bind("JwtSettings", opts))
                    .AddCookie(IdentityConstants.ApplicationScheme, opts => Configuration.Bind("CookieSettings", opts));

            services.Configure<CookiePolicyOptions>(opts =>
            {
                opts.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
                opts.Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
