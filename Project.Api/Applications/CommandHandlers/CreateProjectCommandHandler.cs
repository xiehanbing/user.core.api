using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Api.Applications.Commands;
using Project.Domain.AggregatesModel;

namespace Project.Api.Applications.CommandHandlers
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Domain.AggregatesModel.Project>
    {
        private readonly IProjectRepository _projectRepository;

        public CreateProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<Domain.AggregatesModel.Project> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            //
            await _projectRepository.AddAsync(request.Project);
            await _projectRepository.UnitOfWork.SaveEntitiesAsync();
            return request.Project;
        }
    }
}