using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Wrap;

namespace Resilience
{
    public class ResilientHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;
        //根据 url origin  创建 policy
        private readonly Func<string, IEnumerable<Polly.Policy>> _poliyCreator;
        //把 policy 打包成组合 policy-wraper 进行本地缓存
        private readonly ConcurrentDictionary<string, PolicyWrap> _policyWraps;
        private readonly ILogger<ResilientHttpClient> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ResilientHttpClient(Func<string, IEnumerable<Polly.Policy>> poliyCreator, ILogger<ResilientHttpClient> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = new HttpClient();
            _poliyCreator = poliyCreator;
            _policyWraps = new ConcurrentDictionary<string, PolicyWrap>();
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 执行http post 请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="item"></param>
        /// <param name="authorizationToken"></param>
        /// <param name="requestId"></param>
        /// <param name="authorizationMethod"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> PostAsync<T>(string url, T item, string authorizationToken, string requestId = null,
            string authorizationMethod = "Bearer")
        {
            HttpRequestMessage RequestMessage() => CreateHttpRequestMessage(HttpMethod.Post, url, item);
            var response = DoPostAsync(HttpMethod.Post, url, RequestMessage, authorizationToken, requestId, authorizationMethod);
            return response;
        }

        public Task<HttpResponseMessage> PostAsync(string url, Dictionary<string, string> form, string authorizationToken, string requestId = null,
            string authorizationMethod = "Bearer")
        {
            HttpRequestMessage RequestMessage() => CreateHttpRequestMessage(HttpMethod.Post, url, form);
            var response = DoPostAsync(HttpMethod.Post, url, RequestMessage, authorizationToken, requestId, authorizationMethod);
            return response;
        }

        public  Task<HttpResponseMessage> GetAsync(string url, string authorizationToken = null, string requestId = null,
            string authorizationMethod = "Bearer")
        {
          return  DoGetAsync(HttpMethod.Get, url, authorizationToken, requestId, authorizationMethod);
        }

        private Task<HttpResponseMessage> DoPostAsync(HttpMethod method, string url, Func<HttpRequestMessage> requestMessageAction,
            string authorizationToken, string requestId = null,
            string authorizationMethod = "Bearer")
        {
            if (method != HttpMethod.Post && method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or put", nameof(method));
            }

            var origin = GetOriginFromUri(url);
            return HttpInvokeAync(origin, async () =>
            {
                var requestMessage = requestMessageAction();

                SetAuthorizationHeader(requestMessage);
                if (authorizationToken != null)
                {
                    requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
                }

                if (requestId != null)
                {
                    requestMessage.Headers.Add("x-requestid", requestId);
                }

                var response = await _httpClient.SendAsync(requestMessage);
                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    throw new HttpRequestException(url);
                }
                return response;
            });
        }

        private Task<HttpResponseMessage> DoGetAsync(HttpMethod method, string url,
            string authorizationToken, string requestId = null,
            string authorizationMethod = "Bearer")
        {
            if (method != HttpMethod.Get && method != HttpMethod.Delete)
            {
                throw new ArgumentException("Value must be either get or delete", nameof(method));
            }

            var origin = GetOriginFromUri(url);
            return HttpInvokeAync(origin, async () =>
            {
                var requestMessage = new HttpRequestMessage(method, url);

                SetAuthorizationHeader(requestMessage);
                if (authorizationToken != null)
                {
                    requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
                }

                if (requestId != null)
                {
                    requestMessage.Headers.Add("x-requestid", requestId);
                }

                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    throw new HttpRequestException(url);
                }
                return response;
            });
        
        }

        private async Task<T> HttpInvokeAync<T>(string origin, Func<Task<T>> action)
        {
            var normalizeOrigin = NormalizeOrigin(origin);
            if (!_policyWraps.TryGetValue(normalizeOrigin, out PolicyWrap policyWrap))
            {
                policyWrap = Policy.WrapAsync(_poliyCreator(normalizeOrigin).ToArray());
                _policyWraps.TryAdd(normalizeOrigin, policyWrap);
            }

            return await policyWrap.ExecuteAsync(action,new Context(origin));
        }


        /// <summary>
        /// 格式化 origin
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        private static string NormalizeOrigin(string origin)
        {
            return origin?.Trim().ToLower();
        }
        /// <summary>
        /// 根据uri 获取 origin
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private static string GetOriginFromUri(string uri)
        {
            var url = new Uri(uri);
            var origin = $"{url.Scheme}://{url.DnsSafeHost}:{url.Port}";
            return origin;
        }
        /// <summary>
        /// 设置 验证头 信息
        /// </summary>
        /// <param name="requestMessage"></param>
        private void SetAuthorizationHeader(HttpRequestMessage requestMessage)
        {
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                requestMessage.Headers.Add("Authorization", new List<string>() { authorizationHeader });
            }
        }

        private HttpRequestMessage CreateHttpRequestMessage<T>(HttpMethod method, string url, T item)
        {
            var requestMessage = new HttpRequestMessage(method, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json")
            };
            return requestMessage;
        }

        private HttpRequestMessage CreateHttpRequestMessage<T>(HttpMethod method, string url, Dictionary<string, string> form)
        {
            var requestMessage = new HttpRequestMessage(method, url) { Content = new FormUrlEncodedContent(form) };
            return requestMessage;
        }
    }
}