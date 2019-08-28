using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Api.Applications.Commands;
using Project.Api.Applications.Queries;
using Project.Api.MessageTips;
using Project.Domain.AggregatesModel;

namespace Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : BaseController
    {

        private readonly IMediator _mediator;
        private readonly Applications.Service.IRecommendService _recommendService;
        private readonly IProjectQueries _projectQueries;
        public ProjectsController(IMediator mediator, Applications.Service.IRecommendService recommendService,
            IProjectQueries projectQueries)
        {
            _mediator = mediator;
            _recommendService = recommendService;
            _projectQueries = projectQueries;
        }
        [HttpGet,Route("")]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await _projectQueries.GetProjectByUserId(UserIdentity.UserId);
            return Ok(projects);
        }
        [HttpGet,Route("my/{projectId}")]
        public async Task<IActionResult> GetMyProjectDetail(int projectId)
        {
            var project = await _projectQueries.GetProjectDetail(projectId);
            if (project.UserId == UserIdentity.UserId)
            {
                return Ok(project);
            }

            return BadRequest(ProjectMessageTip.NoAuthViewproject);
        }

        [HttpGet, Route("recommends/{projectId}")]
        public async Task<IActionResult> GetRecommentProjectDetail(int projectId)
        {
            if (await _recommendService.IsProjectInRecommend(projectId, UserIdentity.UserId))
            {
                var project = await _projectQueries.GetProjectDetail(projectId);
                return Ok(project);
            }
            return BadRequest(ProjectMessageTip.NoAuthViewproject);
        }
        [HttpPost,Route("")]
        public async Task<IActionResult> CreateProject(Domain.AggregatesModel.Project project)
        {
            project.UserId = UserIdentity.UserId;
            
            var command = new CreateProjectCommand()
            {
                Project = project
            };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut, Route("view/{projectId}")]
        public async Task<IActionResult> ViewProject(int projectId)
        {
            if (!(await _recommendService.IsProjectInRecommend(projectId, UserIdentity.UserId)))
            {
                return BadRequest(ProjectMessageTip.NoAuthViewproject);
            }


            var command = new ViewProjectCommand()
            {
                UserId = UserIdentity.UserId,
                Avatar = UserIdentity.Avatar,
                UserName = UserIdentity.Name,
                ProjectId = projectId
            };
             await _mediator.Send(command);
            return Ok();
        }

        [HttpPut, Route("join")]
        public async Task<IActionResult> JoinProject([FromBody]ProjectContributor contributor)
        {
            if (!(await _recommendService.IsProjectInRecommend(contributor.ProjectId, UserIdentity.UserId)))
            {
                return BadRequest(ProjectMessageTip.NoAuthViewproject);
            }
            var command = new JoinProjectCommand()
            {
                Contributor = contributor
            };
           await _mediator.Send(command);
            return Ok();
        }
    }
}