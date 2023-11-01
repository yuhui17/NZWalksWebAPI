using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NZWalks.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AlterWalkColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Desciption",
                table: "Walks",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Walks",
                newName: "Desciption");
        }
    }
}
