using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeedStore.Migrations
{
    /// <inheritdoc />
    public partial class SeedAboutPageAndContacts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AboutPage",
                columns: new[] { "Id", "Content" },
                values: new object[] { 1, "" });

            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "Content" },
                values: new object[] { 1, "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AboutPage",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
