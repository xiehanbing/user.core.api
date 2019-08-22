using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Resilience;

namespace User.Core.Identity.Infrastructure
{
    public class ResilienceClientFactory
    {
        private readonly ILogger<ResilientHttpClient> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// 重试次数
        /// </summary>
        private readonly int _retryCout;
        /// <summary>
        /// 熔断之前允许的异常次数
        /// </summary>
        private readonly int _exceptionCountAllowedBeforeBreaking;
        public ResilienceClientFactory(ILogger<ResilientHttpClient> logger, IHttpContextAccessor httpContextAccessor,
             int retryCout, int exceptionCountAllowedBeforeBreaking)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _retryCout = retryCout;
            _exceptionCountAllowedBeforeBreaking = exceptionCountAllowedBeforeBreaking;
        }

        public ResilientHttpClient GetResilientHttpClient() => new ResilientHttpClient(orign=>CreatePolicy(orign), _logger, _httpContextAccessor);

        private Policy[] CreatePolicy(string origin)
        {
            return new Policy[]
            {
                Policy.Handle<HttpRequestException>()
                    .WaitAndRetryAsync(_retryCout,
                        retryAttempt=>TimeSpan.FromSeconds(Math.Pow(2,retryAttempt)),
                        (exception, timeSpan, retryCount, context) =>
                        {
                            var msg =
                                $"第{retryCount}次重试 of {context.PolicyKey} at {context.ExecutionKey}  due to :{exception}.";
                            _logger.LogError(msg);
                            _logger.LogDebug(msg);
                        }),
                Policy.Handle<HttpRequestException>()
                    .CircuitBreakerAsync(_exceptionCountAllowedBeforeBreaking,
                        TimeSpan.FromMinutes(1),
                        (exception, duration) =>
                        {
                            _logger.LogError("熔断器打开");
                        },() =>
                        {
                            _logger.LogError("熔断器关闭");
                        })
            };
        }
    }
}