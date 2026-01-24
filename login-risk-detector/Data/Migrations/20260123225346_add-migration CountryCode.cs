using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace login_risk_detector.Data.Migrations
{
    /// <inheritdoc />
    public partial class addmigrationCountryCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Suceeded",
                table: "LoginEvents",
                newName: "Succeeded");

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "LoginEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "LoginEvents");

            migrationBuilder.RenameColumn(
                name: "Succeeded",
                table: "LoginEvents",
                newName: "Suceeded");
        }
    }
}
