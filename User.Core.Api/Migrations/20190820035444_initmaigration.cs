﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace User.Core.Api.Migrations
{
    public partial class initmaigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserBPFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    OriginFilePath = table.Column<string>(nullable: true),
                    FromatFilePath = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBPFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    Gender = table.Column<byte>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Tel = table.Column<string>(nullable: true),
                    ProvinceId = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    CityId = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    NameCard = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTags",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    Tag = table.Column<string>(maxLength: 100, nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTags", x => new { x.UserId, x.Tag });
                });

            migrationBuilder.CreateTable(
                name: "UserProperties",
                columns: table => new
                {
                    AppUserId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(maxLength: 100, nullable: false),
                    Value = table.Column<string>(maxLength: 100, nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProperties", x => new { x.Key, x.AppUserId, x.Value });
                    table.ForeignKey(
                        name: "FK_UserProperties_Users_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProperties_AppUserId",
                table: "UserProperties",
                column: "AppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBPFiles");

            migrationBuilder.DropTable(
                name: "UserProperties");

            migrationBuilder.DropTable(
                name: "UserTags");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
