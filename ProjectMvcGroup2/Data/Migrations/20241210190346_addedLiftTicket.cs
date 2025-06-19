using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectMvcGroup2.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedLiftTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LiftTicket",
                columns: table => new
                {
                    LiftTicketID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiftTicket", x => x.LiftTicketID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LiftTicket");
        }
    }
}
