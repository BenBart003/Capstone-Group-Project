using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectMvcGroup2.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedLodging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lodging",
                columns: table => new
                {
                    LodgingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomNumber = table.Column<int>(type: "int", nullable: false),
                    BedCount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostPerNight = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lodging", x => x.LodgingID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lodging");
        }
    }
}
