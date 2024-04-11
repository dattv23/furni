using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace furni.Data.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "209a384d-423f-41bb-b049-c24606bff637");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3caea8fd-f428-4237-9476-10632d02a0d8");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Products");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "209a384d-423f-41bb-b049-c24606bff637", "fda69d40-ae54-4ea4-a11c-6910e8bf710e", "admin", "admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3caea8fd-f428-4237-9476-10632d02a0d8", "a8117de7-5dac-47cf-bb3a-6d3353f34ec7", "client", "client" });
        }
    }
}
