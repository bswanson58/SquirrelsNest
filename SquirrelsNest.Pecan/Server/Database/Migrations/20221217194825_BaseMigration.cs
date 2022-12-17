using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SquirrelsNest.Pecan.Server.Database.Migrations
{
    /// <inheritdoc />
    public partial class BaseMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Associations",
                columns: table => new
                {
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssociationId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Associations", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssueNumber = table.Column<long>(type: "bigint", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "date", nullable: false),
                    EnteredById = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssueTypeId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComponentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkflowStateId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedToId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "IssueTypes",
                columns: table => new
                {
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueTypes", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Inception = table.Column<DateTime>(type: "date", nullable: false),
                    RepositoryUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuePrefix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NextIssueNumber = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "Releases",
                columns: table => new
                {
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepositoryLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Releases", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "UserData",
                columns: table => new
                {
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataType = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserData", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoginName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowStates",
                columns: table => new
                {
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowStates", x => x.EntityId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Associations");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "Issues");

            migrationBuilder.DropTable(
                name: "IssueTypes");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Releases");

            migrationBuilder.DropTable(
                name: "UserData");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WorkflowStates");
        }
    }
}
