using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateToBut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBought",
                table: "ToBuy",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "ToBuy",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBought",
                table: "ToBuy");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "ToBuy");
        }
    }
}
