using System.Globalization;
using Cl.Gob.Energia.Ecocarga.Web.Services;
using Cl.Gob.Energia.Ecocarga.Web.Services.Interfaces;
using Cl.Gob.Energia.Ecocarga.Web.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cl.Gob.Energia.Ecocarga.Web
{
    public class Startup
    {
        private readonly IHostingEnvironment env;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    if (env.IsDevelopment())
                    {
                        options.Cookie.Domain = "localhost";
                    }
                    else
                    {
                        options.Cookie.Domain = Configuration.Get<AppSettings>().Authentication.CookieDomain;
                    }
                });

            services.Configure<AppSettings>(Configuration);
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(0, new CustomBinderProvider());
                options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => $"El campo {x} debe ser numérico.");
                options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => "Campo requerido.");
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => $"El valor '{x}' es inválido");
                options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => $"El valor '{x}' no es válido.");
                options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "El campo debe ser numérico.");
                options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x => $"El valor '{x}' no es válido.");
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            string connectionString = Configuration["ConnectionString"];
            services.AddDbContext<Data.Models.EcocargaContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IAuthorizationLogicService, AuthorizationLogicService>();
            services.AddHttpContextAccessor();
            services.AddTransient<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //ModelBinders.Binders.Add(typeof(double), new DecimalModelBinder());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var cultureInfo = new CultureInfo("es-CL");

            UpdateDatabase(app);

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}/{id?}");
            });
        }

        private void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<Data.Models.EcocargaContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
