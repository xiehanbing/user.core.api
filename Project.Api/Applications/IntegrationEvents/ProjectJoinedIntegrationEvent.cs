using Project.Domain.AggregatesModel;

namespace Project.Api.Applications.IntegrationEvents
{
    public class ProjectJoinedIntegrationEvent
    {
        public string Company { get; set; }
        public string Introduction { get; set; }
        public Domain.AggregatesModel.ProjectContributor Contributor { get; set; }
    }
}