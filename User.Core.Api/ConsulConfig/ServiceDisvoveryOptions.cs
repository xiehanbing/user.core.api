﻿namespace User.Core.Api.ConsulConfig
{
    public class ServiceDisvoveryOptions
    {

        public string ServiceName { get; set; }

        public ConsulOptions Consul { get; set; }
    }
}