using System.Threading.Tasks;
using User.Core.Identity.Dtos;

namespace User.Core.Identity.Services
{
    public interface IUserService
    {
        /// <summary>
        /// 检查手机号 是否已注册 如果没有注册 则添加
        /// </summary>
        /// <param name="phone">手机号</param>
        Task<UserInfo> CheckOrCreateAsync(string phone);
    }
}