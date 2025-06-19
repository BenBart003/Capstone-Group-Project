using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectMvcGroup2.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedRents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rents",
                columns: table => new
                {
                    RentsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EquipmentRentalID = table.Column<int>(type: "int", nullable: false),
                    ERentStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ERentEndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rents", x => x.RentsID);
                    table.ForeignKey(
                        name: "FK_Rents_AspNetUsers_GuestId",
                        column: x => x.GuestId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rents_EquipmentRental_EquipmentRentalID",
                        column: x => x.EquipmentRentalID,
                        principalTable: "EquipmentRental",
                        principalColumn: "EquipmentRentalID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rents_EquipmentRentalID",
                table: "Rents",
                column: "EquipmentRentalID");

            migrationBuilder.CreateIndex(
                name: "IX_Rents_GuestId",
                table: "Rents",
                column: "GuestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rents");
        }
    }
}
