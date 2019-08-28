using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Domain.AggregatesModel;
using Project.Domain.SeedWork;
using ProjectEntity = Project.Domain.AggregatesModel.Project;
namespace Project.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        public IUnitOfWork UnitOfWork => _context;
        private readonly ProjectContext _context;

        public ProjectRepository(ProjectContext context)
        {
            _context = context;
        }
        public async Task<ProjectEntity> AddAsync(ProjectEntity project)
        {
            if (project.IsTransient())
            {
                var data = await _context.Projects.AddAsync(project);
                return data.Entity;
            }
            return project;
        }

        public async Task<ProjectEntity> GetAsync(int id)
        {
            var project = await _context.Projects.Include(p => p.Properties)
                  .Include(p => p.Viewers)
                  .Include(p => p.Contributors)
                  .Include(p => p.VisibleRule)
                  .SingleOrDefaultAsync(o => o.Id == id);
            return project;
        }

        public ProjectEntity UpdateAsync(ProjectEntity project)
        {
            return _context.Projects.Update(project).Entity;
        }
    }
}