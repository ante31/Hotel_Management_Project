using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelMS.DataAccess.Persistence.Migrations
{
    public partial class deleteForReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "deleted",
                table: "Reservation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "deleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deleted",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "deleted",
                table: "AspNetUsers");
        }
    }
}
