using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectMvcGroup2.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedLodgingDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LodgingDates",
                columns: table => new
                {
                    LodgingDatesID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LodgingID = table.Column<int>(type: "int", nullable: false),
                    CheckInDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CheckOutDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LodgingDates", x => x.LodgingDatesID);
                    table.ForeignKey(
                        name: "FK_LodgingDates_AspNetUsers_GuestId",
                        column: x => x.GuestId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LodgingDates_Lodging_LodgingID",
                        column: x => x.LodgingID,
                        principalTable: "Lodging",
                        principalColumn: "LodgingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LodgingDates_GuestId",
                table: "LodgingDates",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_LodgingDates_LodgingID",
                table: "LodgingDates",
                column: "LodgingID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LodgingDates");
        }
    }
}
