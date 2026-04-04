using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevJobsAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddFormToApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AppliedAt",
                table: "JobApplications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CVUrl",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppliedAt",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "CVUrl",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "JobApplications");
        }
    }
}
