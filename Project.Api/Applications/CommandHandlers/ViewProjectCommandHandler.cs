using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Api.Applications.Commands;
using Project.Domain.AggregatesModel;
using Project.Domain.Exceptions;

namespace Project.Api.Applications.CommandHandlers
{
    public class ViewProjectCommandHandler : IRequestHandler<ViewProjectCommand>
    {
        private readonly IProjectRepository _projectRepository;
        public ViewProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<Unit> Handle(ViewProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetAsync(request.ProjectId);
            if (project == null)
            {
                throw new ProjectDomainException("project not found:" + request.ProjectId);
            }
            if (project.UserId == request.UserId)
            {
                throw new ProjectDomainException("you cannot view your own project ");
            }
            project.AddViewer(request.UserId,request.UserName,request.Avatar);
            await _projectRepository.UnitOfWork.SaveEntitiesAsync();
            return new Unit();
        }
    }
}