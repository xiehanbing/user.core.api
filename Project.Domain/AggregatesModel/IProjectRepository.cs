using System.Threading.Tasks;
using Project.Domain.SeedWork;
using  ProjectEntity=Project.Domain.AggregatesModel.Project;
namespace Project.Domain.AggregatesModel
{
    public interface IProjectRepository
    {
        IUnitOfWork UnitOfWork { get; }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        Task<ProjectEntity> AddAsync(ProjectEntity project);
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProjectEntity> GetAsync(int id);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        ProjectEntity UpdateAsync(ProjectEntity project);
    }
}