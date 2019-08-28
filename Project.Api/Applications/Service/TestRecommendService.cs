using System.Threading.Tasks;

namespace Project.Api.Applications.Service
{
    public class TestRecommendService:IRecommendService
    {
        public  Task<bool> IsProjectInRecommend(int projectId, int userId)
        {
            return Task.FromResult(true);
        }
    }
}