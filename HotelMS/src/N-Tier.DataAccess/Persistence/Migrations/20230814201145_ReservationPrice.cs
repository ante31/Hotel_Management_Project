using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelMS.DataAccess.Persistence.Migrations
{
    public partial class ReservationPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Reservation",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Reservation");
        }
    }
}
