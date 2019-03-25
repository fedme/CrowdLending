using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdLending.Database;
using CrowdLending.Models;
using CrowdLending.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrowdLending.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IInvestmentService _investmentService;
        private readonly UserManager<UserEntity> _userManager;

        public ProjectsController(
            IProjectService projectService, 
            IInvestmentService investmentService,
            UserManager<UserEntity> userManager
        )
        {
            _projectService = projectService;
            _investmentService = investmentService;
            _userManager = userManager;
        }

        [HttpGet(Name = nameof(GetProjects))]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<ProjectEntity>>> GetProjects()
        {
            var projects = await _projectService.GetAllProjectsWithOwnersAsync();
            return Ok(projects);
        }

        [HttpGet("{projectId}", Name = nameof(GetProjectById))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProjectEntity>> GetProjectById(Guid projectId)
        {
            var project = await _projectService.GetProjectAsync(projectId);
            if (project == null)
                return NotFound();
            return project;
        }

        [HttpGet("{projectId}/investments", Name = nameof(GetProjectInvestments))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<InvestmentEntity>> GetProjectInvestments(Guid projectId)
        {
            var project = await _projectService.GetProjectAsync(projectId);
            if (project == null)
                return NotFound();

            var investments = await _investmentService.GetInvestmentsByProjectIdAsync(project.Id);

            return Ok(investments);
        }

        // POST /projects/{projectId}/investments
        [Authorize]
        [HttpPost("{projectId}/investments", Name = nameof(CreateInvestmentForProject))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(201)]
        public async Task<ActionResult> CreateInvestmentForProject(
            Guid projectId, [FromBody] InvestmentForm form)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null) return Unauthorized();

            var project = await _projectService.GetProjectAsync(projectId);
            if (project == null) return NotFound();

            var (isValidInvestment, errorMsg) = await _investmentService.IsProjectInvestmentValid(project, user, form.Amount);

            if (isValidInvestment)
            {
                // Create investment
                var investmentId = await _investmentService.CreateInvestmentAsync(user.Id, project.Id, form.Amount);
                return Created(investmentId.ToString(), null); // TODO: return url to created investment
            }

            return BadRequest(errorMsg);
            
        }
    }
}
