using System.Net;

namespace User.Core.Api.ConsulConfig
{
    public class DnsEndpoint
    {
        public string Address { get; set; }

        public int Port { get; set; }

        public IPEndPoint ToIpEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(Address), Port);
        }
    }
}