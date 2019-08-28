using System.Threading.Tasks;

namespace Project.Api.Applications.Queries
{
    public interface IProjectQueries
    {

        Task<dynamic> GetProjectByUserId(int userId);


        Task<dynamic> GetProjectDetail( int projectId);
    }
}