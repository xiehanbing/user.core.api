namespace User.Core.Api.ConsulConfig
{
    public class ConsulOptions
    {
        public string HttpEndpoint { get; set; }

        public DnsEndpoint DnsEndpoint { get; set; }
    }
}