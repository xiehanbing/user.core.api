namespace Project.Api.Dtos
{
    public class ServiceDisvoveryOptions
    {

        public string ServiceName { get; set; }

        public ConsulOptions Consul { get; set; }
    }
}