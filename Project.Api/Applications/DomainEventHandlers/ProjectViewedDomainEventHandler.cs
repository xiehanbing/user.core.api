using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using MediatR;
using Project.Api.Applications.IntegrationEvents;
using Project.Domain.Events;

namespace Project.Api.Applications.DomainEventHandlers
{
    public class ProjectViewedDomainEventHandler : INotificationHandler<ProjectViewEvent>
    {
        private readonly ICapPublisher _capPublisher;

        public ProjectViewedDomainEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }
        public async Task Handle(ProjectViewEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectViewedIntegrationEvent
            {
                Company = notification.Company,
                Introduction = notification.Introduction,
                Viewer = notification.Viewer
            };
            await _capPublisher.PublishAsync("project.api.view.project", @event, cancellationToken: cancellationToken);
        }
    }
}