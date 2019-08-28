using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using MediatR;
using Project.Api.Applications.IntegrationEvents;
using Project.Domain.Events;

namespace Project.Api.Applications.DomainEventHandlers
{
    public class ProjectCreatedDomainEventHandler : INotificationHandler<ProjecCreatedtEvent>
    {
        private readonly ICapPublisher _capPublisher;
        public ProjectCreatedDomainEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }
        public async Task Handle(ProjecCreatedtEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectCreatedIntegrationEvent
            {
                ProjectId = notification.Project.Id,
                UserId = notification.Project.UserId,
                CreatedTime = notification.Project.CreatedTime
            };
            await _capPublisher.PublishAsync("project.api.create.project", @event, cancellationToken: cancellationToken);
        }
    }
}