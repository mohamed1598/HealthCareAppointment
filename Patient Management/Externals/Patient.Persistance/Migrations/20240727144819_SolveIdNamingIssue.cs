using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patient.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SolveIdNamingIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "OutboxMessages",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "OutboxMessages",
                newName: "id");
        }
    }
}
