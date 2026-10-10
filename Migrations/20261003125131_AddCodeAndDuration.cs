using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi1.Migrations
{
    /// <inheritdoc />
    public partial class AddCodeAndDuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Specialties",
                newName: "Code");

            migrationBuilder.AddColumn<int>(
                name: "DurationYears",
                table: "Specialties",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationYears",
                table: "Specialties");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Specialties",
                newName: "Duration");
        }
    }
}
