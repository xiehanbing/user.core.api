using MediatR;

namespace Project.Domain.Events
{
    public class ProjecCreatedtEvent:INotification
    {
        public Domain.AggregatesModel.Project Project { get; set; }
    }
}