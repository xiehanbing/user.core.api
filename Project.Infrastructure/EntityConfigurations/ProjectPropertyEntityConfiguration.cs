using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;

namespace Project.Infrastructure.EntityConfigurations
{
    public class ProjectPropertyEntityConfiguration:IEntityTypeConfiguration<ProjectProperty>
    {
        public void Configure(EntityTypeBuilder<ProjectProperty> builder)
        {
            builder.ToTable("ProjectPropertys")
                .HasKey(p=>new{p.ProjectId,p.Key,p.Value});
            builder.Property(p => p.Key).HasMaxLength(100);
            builder.Property(p => p.Value).HasMaxLength(100);


        }
    }
}