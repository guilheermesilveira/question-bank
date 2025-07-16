using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuestionBank.Infra.Migrations
{
    public partial class AddDefaultAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var password = "$argon2id$v=19$m=16,t=2,p=1$NEFHVUN2bnFGNzliSEpUQQ$+wXX+SRhO9nYnLTeEFGU0g";

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "Email", "Password", "IsAdmin" },
                values: new object[,]
                {
                    { 1, "Administrator", "admin@admin.com", password, true }
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1
            );
        }
    }
}
