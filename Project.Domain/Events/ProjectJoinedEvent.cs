using MediatR;

namespace Project.Domain.Events
{
    public class ProjectJoinedEvent: INotification
    {
        public string Company { get; set; }
        public string Introduction { get; set; }
        public Domain.AggregatesModel.ProjectContributor Contributor { get; set; }
    }
}