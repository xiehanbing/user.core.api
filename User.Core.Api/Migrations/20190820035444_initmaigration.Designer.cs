﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using User.Core.Api.Data;

namespace User.Core.Api.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20190820035444_initmaigration")]
    partial class initmaigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("User.Core.Api.Models.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<string>("Avatar");

                    b.Property<string>("City");

                    b.Property<string>("CityId");

                    b.Property<string>("Company");

                    b.Property<string>("Email");

                    b.Property<byte>("Gender");

                    b.Property<string>("Name");

                    b.Property<string>("NameCard");

                    b.Property<string>("Phone");

                    b.Property<string>("Province");

                    b.Property<string>("ProvinceId");

                    b.Property<string>("Tel");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("User.Core.Api.Models.BpFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("FileName");

                    b.Property<string>("FromatFilePath");

                    b.Property<string>("OriginFilePath");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("UserBPFiles");
                });

            modelBuilder.Entity("User.Core.Api.Models.UserProperty", b =>
                {
                    b.Property<string>("Key")
                        .HasMaxLength(100);

                    b.Property<int>("AppUserId");

                    b.Property<string>("Value")
                        .HasMaxLength(100);

                    b.Property<string>("Text");

                    b.HasKey("Key", "AppUserId", "Value");

                    b.HasIndex("AppUserId");

                    b.ToTable("UserProperties");
                });

            modelBuilder.Entity("User.Core.Api.Models.UserTag", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("Tag")
                        .HasMaxLength(100);

                    b.Property<DateTime>("CreatedTime");

                    b.HasKey("UserId", "Tag");

                    b.ToTable("UserTags");
                });

            modelBuilder.Entity("User.Core.Api.Models.UserProperty", b =>
                {
                    b.HasOne("User.Core.Api.Models.AppUser")
                        .WithMany("Properties")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
