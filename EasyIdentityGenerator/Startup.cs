using System.Net.Http;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using CurrieTechnologies.Razor.Clipboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EasyIdentityGenerator.Data.Context;
using EasyIdentityGenerator.Data.Models;
using EasyIdentityGenerator.Data.Services.Implementation;
using EasyIdentityGenerator.Data.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;

namespace EasyIdentityGenerator
{
    public class Startup
    {
        private readonly IHostEnvironment _env;
        public Startup(IHostEnvironment env)
        {
            _env = env;
            //Configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddClipboard();
            services.AddBlazorise(options =>
                {
                    options.ChangeTextOnKeyPress = true;
                })
                .AddBootstrapProviders()
                .AddFontAwesomeIcons();
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
            }).AddAzureSignalR(options =>
            {
                options.ServerStickyMode =
                    Microsoft.Azure.SignalR.ServerStickyMode.Required;
            });

            services.AddScoped<IIdentityGenerator, IdentityGenerator>();
            services.AddScoped<IPasswordHasherMvc, PasswordHasherMvc>();
            services.AddScoped<IFileReader, FileReader>();
            services.AddScoped<IDataGenerator, DataGenerator>();
            services.AddHttpClient<IHttpService<RandomUser>, RandomUserHttpService>()
                .AddPolicyHandler(HttpRetryPolicy());
            services.AddHttpClient<IHttpService<RandomPassword>, RandomPasswordHttpService>()
                .AddPolicyHandler(HttpRetryPolicy());
            services.AddDbContext<EasyIdentityDbContext>(opt => opt.UseInMemoryDatabase("EasyIdentityGeneratorDb"));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<EasyIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 0;
            });
        }

        private static AsyncRetryPolicy<HttpResponseMessage> HttpRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .Or<Newtonsoft.Json.JsonReaderException>()
                .RetryAsync(3);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
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

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseWebSockets();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None
            });

            app.ApplicationServices
                .UseBootstrapProviders()
                .UseFontAwesomeIcons();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
