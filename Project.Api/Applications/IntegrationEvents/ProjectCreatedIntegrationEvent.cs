using System;

namespace Project.Api.Applications.IntegrationEvents
{
    public class ProjectCreatedIntegrationEvent
    {
        public int  ProjectId { get; set; }
        public int  UserId { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}