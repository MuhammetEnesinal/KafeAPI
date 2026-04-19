using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KafeAPI.Persistence.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class creatrewiewtablefix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CafeInfoId",
                table: "Reviews");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CafeInfoId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
