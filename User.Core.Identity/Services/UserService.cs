using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Resilience;
using User.Core.Identity.Dtos;

namespace User.Core.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly string _userServiceUrl;
        private readonly IHttpClient _httpClient;
        private readonly ILogger<UserService> _logger;
        public UserService(IHttpClient httpClient,IDnsQuery dnsQuery,IOptions<Dtos.ServiceDisvoveryOptions> serviceDisvoveryOptions, ILogger<UserService> logger)
        {
            _httpClient = httpClient;
            var address = dnsQuery.ResolveService("service.consul",serviceDisvoveryOptions.Value.UserServiceName);
            var addressList = address.First().AddressList;
            var host = addressList.Any()? addressList.First().ToString(): address.First().HostName?.TrimEnd('.');
            var port = address.First().Port;
            _userServiceUrl = $"http://{host}:{port}";
            _logger = logger;
            Console.WriteLine("current http url:"+_userServiceUrl);
        }
        public async Task<UserInfo> CheckOrCreate(string phone)
        {
            var form = new Dictionary<string, string>() { { "phone", phone } };
            try
            {
                var response = await _httpClient.PostAsync(_userServiceUrl + "/api/user/check-or-create?phone=" + phone, form);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var userInfo = await response.Content.ReadAsStringAsync();
                    //int.TryParse(userInfo, out int userId);
                    return JsonConvert.DeserializeObject<UserInfo>(userInfo);
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("CheckOrCreate 在 重试之后 失败，"+ex.Message+ex.StackTrace);
                throw ex;
            }
          
            //todo 异常处理  polly  进行重试 熔断 限流 等 相关 操作
            //throw new System.NotImplementedException();
        }
    }
}