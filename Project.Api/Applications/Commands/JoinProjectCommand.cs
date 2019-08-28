using MediatR;

namespace Project.Api.Applications.Commands
{
    public class JoinProjectCommand:IRequest
    {
        public Domain.AggregatesModel.ProjectContributor Contributor { get; set; }
    }
}