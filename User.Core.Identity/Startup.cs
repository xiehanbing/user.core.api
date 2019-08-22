using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DnsClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resilience;
using User.Core.Identity.Authentication;
using User.Core.Identity.Dtos;
using User.Core.Identity.Infrastructure;
using User.Core.Identity.Services;

namespace User.Core.Identity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddExtensionGrantValidator<SmsAuthCodeValidator>()
                .AddDeveloperSigningCredential()
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryIdentityResources(Config.GetIdentityResources());

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthCodeService, TestAuthCodeService>();
            //services.AddScoped(typeof(HttpClient));
            //注册全局单例 ResilienceClientFactory
            services.AddSingleton(typeof(ResilienceClientFactory), sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ResilientHttpClient>>();
                var httpcontextAccesser = sp.GetRequiredService<IHttpContextAccessor>();
                var retryCount = 5;
                var exceptionCountAllowedBeforeBreaking = 5;
                var factory = new ResilienceClientFactory(logger, httpcontextAccesser, retryCount,
                    exceptionCountAllowedBeforeBreaking);
                return factory;
            });

            //注册全局单例 httpclient
            services.AddSingleton<IHttpClient>(sp => sp.GetRequiredService<ResilienceClientFactory>().GetResilientHttpClient());
            services.Configure<Dtos.ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));

            services.AddSingleton<IDnsQuery>(p =>
            {
                var serviceConfiguration = p.GetService<IOptions<ServiceDisvoveryOptions>>().Value;
                return new LookupClient(serviceConfiguration.Consul.DnsEndpoint.ToIpEndPoint());
            });
           
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseIdentityServer();
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
