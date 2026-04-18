using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeedStore.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerCommentAndFixTakenByEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Employees_TakenByEmployeeId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_TakenByEmployeeId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TakenByEmployeeId1",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "CustomerComment",
                table: "Orders",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerComment",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "TakenByEmployeeId1",
                table: "Orders",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TakenByEmployeeId1",
                table: "Orders",
                column: "TakenByEmployeeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Employees_TakenByEmployeeId1",
                table: "Orders",
                column: "TakenByEmployeeId1",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
