using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectMvcGroup2.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedTicketDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TicketDate",
                columns: table => new
                {
                    TicketDateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuestId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LiftTicketID = table.Column<int>(type: "int", nullable: false),
                    PassStartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PassEndDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketDate", x => x.TicketDateID);
                    table.ForeignKey(
                        name: "FK_TicketDate_AspNetUsers_GuestId",
                        column: x => x.GuestId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketDate_LiftTicket_LiftTicketID",
                        column: x => x.LiftTicketID,
                        principalTable: "LiftTicket",
                        principalColumn: "LiftTicketID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketDate_GuestId",
                table: "TicketDate",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketDate_LiftTicketID",
                table: "TicketDate",
                column: "LiftTicketID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketDate");
        }
    }
}
