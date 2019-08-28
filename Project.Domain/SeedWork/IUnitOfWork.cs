using System.Threading;
using System.Threading.Tasks;

namespace Project.Domain.SeedWork
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// 保存实体
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveEntitiesAsync();
    }
}