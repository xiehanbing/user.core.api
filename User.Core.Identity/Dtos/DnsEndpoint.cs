using System.Net;

namespace User.Core.Identity.Dtos
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