using Microsoft.EntityFrameworkCore;

namespace Project.Infrastructure.EntityConfigurations
{
    public static class InitEntityConfiguration
    {
        public static void InitEntityConfig(this ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new ProjectEntityConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectContext).Assembly);

        }
    }
}