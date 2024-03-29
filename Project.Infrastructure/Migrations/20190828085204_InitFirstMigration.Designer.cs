﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Project.Infrastructure;

namespace Project.Infrastructure.Migrations
{
    [DbContext(typeof(ProjectContext))]
    [Migration("20190828085204_InitFirstMigration")]
    partial class InitFirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Project.Domain.AggregatesModel.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AreaId");

                    b.Property<string>("Avatar");

                    b.Property<int>("BrokerageOptions");

                    b.Property<int>("CityId");

                    b.Property<string>("CityName");

                    b.Property<string>("Company");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<decimal>("FinMoney");

                    b.Property<decimal>("FinPercentage");

                    b.Property<int>("FinStage");

                    b.Property<string>("Income");

                    b.Property<string>("Introduction");

                    b.Property<string>("OnPlatform");

                    b.Property<string>("OriginBpFile");

                    b.Property<int>("ProvinceId");

                    b.Property<string>("ProvinceName");

                    b.Property<int>("ReferenceId");

                    b.Property<DateTime>("RegisterTime");

                    b.Property<decimal>("Revenue");

                    b.Property<string>("ShowSecurityInfo");

                    b.Property<int>("SourceId");

                    b.Property<string>("Tags");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<int>("UserId");

                    b.Property<decimal>("Valution");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectContributor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Avatar");

                    b.Property<int>("ContributorType");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<bool>("IsCloser");

                    b.Property<int>("ProjectId");

                    b.Property<int>("UserId");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectContributors");
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectProperty", b =>
                {
                    b.Property<int>("ProjectId");

                    b.Property<string>("Key")
                        .HasMaxLength(100);

                    b.Property<string>("Value")
                        .HasMaxLength(100);

                    b.Property<string>("Text");

                    b.HasKey("ProjectId", "Key", "Value");

                    b.ToTable("ProjectPropertys");
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectViewer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Avatar");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<int>("ProjectId");

                    b.Property<int>("UserId");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectViewers");
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectVisibleRule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ProjectId");

                    b.Property<string>("Tags");

                    b.Property<bool>("Visible");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId")
                        .IsUnique();

                    b.ToTable("ProjectVisibleRules");
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectContributor", b =>
                {
                    b.HasOne("Project.Domain.AggregatesModel.Project")
                        .WithMany("Contributors")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectProperty", b =>
                {
                    b.HasOne("Project.Domain.AggregatesModel.Project")
                        .WithMany("Properties")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectViewer", b =>
                {
                    b.HasOne("Project.Domain.AggregatesModel.Project")
                        .WithMany("Viewers")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectVisibleRule", b =>
                {
                    b.HasOne("Project.Domain.AggregatesModel.Project")
                        .WithOne("VisibleRule")
                        .HasForeignKey("Project.Domain.AggregatesModel.ProjectVisibleRule", "ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
