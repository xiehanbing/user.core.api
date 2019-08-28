using MediatR;

namespace Project.Api.Applications.Commands
{
    public class CreateProjectCommand: IRequest<Domain.AggregatesModel.Project>
    {
        public Domain.AggregatesModel.Project Project { get; set; }
    }
}