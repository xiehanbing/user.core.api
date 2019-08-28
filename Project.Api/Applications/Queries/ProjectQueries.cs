using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace Project.Api.Applications.Queries
{
    public class ProjectQueries : IProjectQueries
    {
        private readonly string _connectionString;

        public ProjectQueries(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<dynamic> GetProjectByUserId(int userId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var result = await conn.QueryAsync<dynamic>("SELECT Id,Avatar,Company,FinStage,Introduction,Tags,ShowSecurityInfo,CreatedTime FROM  dbo.Projects WHERE UserId=@userId",new {userId});
                return result;
            }
        }

        public async Task<dynamic> GetProjectDetail( int projectId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var result = await conn.QueryAsync<dynamic>(@"
SELECT project.*,prule.Tags,prule.Visible FROM  dbo.Projects project WITH(NOLOCK) 
INNER JOIN  dbo.ProjectVisibleRules prule WITH(NOLOCK)
ON  prule.ProjectId = project.Id
WHERE project.Id=@projectId 
--AND project.UserId=@userId
",new {projectId});
                return result;
            }
        }
    }
}