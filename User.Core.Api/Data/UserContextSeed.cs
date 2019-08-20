using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using User.Core.Api.Models;

namespace User.Core.Api.Data
{
    /// <summary>
    /// 初始化数据类和方法
    /// </summary>
    public class UserContextSeed
    {
        private ILogger<UserContextSeed> _logger;

        public UserContextSeed(Logger<UserContextSeed> logger)
        {
            _logger = logger;
        }
        public static async Task SeedAsync(IApplicationBuilder builder, ILoggerFactory loggerFactory,int? retry=0)
        {
            var retryForAvaiability = retry??0;
            try
            {
                using (var scope=builder.ApplicationServices.CreateScope())
                {
                    var context = (UserContext) scope.ServiceProvider.GetService(typeof(UserContext));
                    var logger =
                        (ILogger<UserContextSeed>) scope.ServiceProvider.GetService(typeof(ILogger<UserContextSeed>));
                    logger.LogDebug("Begin UserContextSeed SeedAsync");
                    context.Database.Migrate();
                    if (!context.Users.Any())
                    {
                        context.Users.Add(new AppUser() {Name = "Drama"});
                        context.SaveChanges();
                    }
                    logger.LogDebug("End UserContextSeed SeedAsync");
                }
            }
            catch (Exception ex)
            {
                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;
                    var logger = loggerFactory.CreateLogger(typeof(UserContextSeed));
                    logger.LogError(ex.Message);
                    await SeedAsync(builder, loggerFactory, retryForAvaiability);
                }
            }

        }
    }
}