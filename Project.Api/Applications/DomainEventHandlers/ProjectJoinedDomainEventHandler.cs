using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using MediatR;
using Project.Api.Applications.IntegrationEvents;
using Project.Domain.Events;

namespace Project.Api.Applications.DomainEventHandlers
{
    public class ProjectJoinedDomainEventHandler: INotificationHandler<ProjectJoinedEvent>
    {
        private readonly ICapPublisher _capPublisher;

        public ProjectJoinedDomainEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }
        public async Task Handle(ProjectJoinedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectJoinedIntegrationEvent
            {
                Company = notification.Company,
                Introduction = notification.Introduction,
                Contributor = notification.Contributor
            };
            await _capPublisher.PublishAsync("project.api.join.project", @event, cancellationToken: cancellationToken);
        }
    }
}