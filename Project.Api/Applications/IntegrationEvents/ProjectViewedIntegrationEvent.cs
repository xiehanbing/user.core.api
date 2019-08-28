using Project.Domain.AggregatesModel;

namespace Project.Api.Applications.IntegrationEvents
{
    public class ProjectViewedIntegrationEvent
    {
        public string  Company { get; set; }
        public string Introduction { get; set; }
        public ProjectViewer Viewer { get; set; }
    }
}