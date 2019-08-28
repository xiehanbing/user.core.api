using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Project.Api.Applications.Queries;
using Project.Api.Applications.Service;
using Project.Api.Dtos;
using Project.Domain.AggregatesModel;
using Project.Domain.SeedWork;
using Project.Infrastructure;
using Project.Infrastructure.Repositories;

namespace Project.Api
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
            services.AddMediatR(typeof(Startup));
            services.AddDbContext<Infrastructure.ProjectContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), sql =>
                    {
                        //可在当前项目 下 创建 migration
                        //sql.MigrationsAssembly(typeof(Startup).Assembly.FullName);
                    });
            });
            services.Configure<Dtos.ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            services.AddSingleton<IConsulClient>(p => new ConsulClient(cfg =>
            {
                var serviceConfiguration = p.GetService<IOptions<ServiceDisvoveryOptions>>().Value;
                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                {
                    cfg.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                }
            }));
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Audience = "project_api";
                    options.Authority = "http://localhost:5004";

                });

            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IUnitOfWork, ProjectContext>();

            services.AddScoped<IProjectQueries, ProjectQueries>(sp =>
            {
                return new ProjectQueries(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IRecommendService, TestRecommendService>();

            services.AddCap(options =>
            {
                //options.UseEntityFramework<UserContext>();
                //cap usesqlserver 此方法默认使用的数据库Schema为Cap 这种方式  最低要求为sqlserver 2012 （(因为使用了Dashboard的sql查询语句使用了Format新函数）
                //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

                // 此方法可以指定是否使用sql server2008,数据库Schema,链接字符串
                options.UseSqlServer(sqloptions =>
                {
                    //数据库连接字符串
                    sqloptions.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                    sqloptions.UseSqlServer2008();

                });
                options.UseRabbitMQ(rb =>
                {
                    rb.HostName = "127.0.0.1";
                    rb.UserName = "guest";
                    rb.Password = "guest";
                    rb.Port = 5672;
                });
                options.UseDashboard();
                options.UseDiscovery(d =>
                {
                    d.DiscoveryServerHostName = "localhost";
                    d.DiscoveryServerPort = 8500;
                    d.CurrentNodePort = 5000;
                    d.NodeId = "1";
                    d.NodeName = "CAP NO.2  Project Node ";
                });
                //options.FailedRetryCount = 2;
                //options.FailedRetryInterval = 5;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime,
            IOptions<ServiceDisvoveryOptions> serviceOptions,
            IConsulClient consul)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            //启动时候 注册服务
            applicationLifetime.ApplicationStarted.Register(() => ResiterService(app, serviceOptions, consul, applicationLifetime));

            //todo  停止时 移除服务
            applicationLifetime.ApplicationStopped.Register(() => DeRegisterService(app, serviceOptions, consul));
            app.UseMvc();
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="app"></param>
        /// <param name="serviceOptions"></param>
        /// <param name="consul"></param>
        /// <param name="applicationLifetime"></param>
        private void ResiterService(IApplicationBuilder app, IOptions<ServiceDisvoveryOptions> serviceOptions, IConsulClient consul, IApplicationLifetime applicationLifetime)
        {

            var addresses = GetCurrentUri(app);
            if (addresses == null) return;
            foreach (var address in addresses)
            {
                var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";
                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    Interval = TimeSpan.FromSeconds(30),
                    HTTP = new Uri(address, "HealthCheck").OriginalString
                };

                var registration = new AgentServiceRegistration()
                {
                    Checks = new[] { httpCheck },
                    Address = address.Host,

                    ID = serviceId,
                    Name = serviceOptions.Value.ServiceName,
                    Port = address.Port
                };
                consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();

            }

        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="app"></param>
        /// <param name="serviceOptions"></param>
        /// <param name="consul"></param>
        private void DeRegisterService(IApplicationBuilder app, IOptions<ServiceDisvoveryOptions> serviceOptions,
            IConsulClient consul)
        {
            var addresses = GetCurrentUri(app);
            if (addresses == null) return;
            foreach (var address in addresses)
            {
                var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";
                consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
            }
        }
        /// <summary>
        /// //从当前启动的url 中拿到url
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        private IEnumerable<Uri> GetCurrentUri(IApplicationBuilder app)
        {
            var features = app.Properties["server.Features"] as FeatureCollection;
            var address = features?.Get<IServerAddressesFeature>().Addresses.Select(p => new Uri(p));
            return address;
        }
    }
}
