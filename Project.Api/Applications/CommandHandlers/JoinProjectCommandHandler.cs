using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Api.Applications.Commands;
using Project.Domain.AggregatesModel;
using Project.Domain.Exceptions;

namespace Project.Api.Applications.CommandHandlers
{
    public class JoinProjectCommandHandler : IRequestHandler<JoinProjectCommand>
    {
        private readonly IProjectRepository _projectRepository;
        public JoinProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<Unit>Handle(JoinProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetAsync(request.Contributor.ProjectId);
            if (project == null)
            {
                throw new ProjectDomainException("project not found:" + request.Contributor.ProjectId);
            }

            if (project.UserId == request.Contributor.UserId)
            {
                throw new ProjectDomainException("you cannot join your own project ");
            }
            project.AddContributor(request.Contributor);
            await _projectRepository.UnitOfWork.SaveEntitiesAsync();
            return new Unit();
        }
    }
}