using MediatR;

namespace Project.Domain.Events
{
    public class ProjectViewEvent: INotification
    {
        public string  Company { get; set; }
        public string Introduction { get; set; }
        public Domain.AggregatesModel.ProjectViewer Viewer { get; set; }
    }
}