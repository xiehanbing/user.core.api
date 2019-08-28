﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;

namespace Project.Infrastructure.EntityConfigurations
{
    public class ProjectViewerEntityConfiguration:IEntityTypeConfiguration<ProjectViewer>
    {
        public void Configure(EntityTypeBuilder<ProjectViewer> builder)
        {
            builder.ToTable("ProjectViewers")
                .HasKey(p=>p.Id);

        }
    }
}