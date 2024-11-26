using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    MaxQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    DishType = table.Column<int>(type: "INTEGER", nullable: false),
                    ServingTime = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "Id", "DishType", "MaxQuantity", "Name", "ServingTime" },
                values: new object[,]
                {
                    { 1, 1, 1, "steak", 1 },
                    { 2, 2, 2147483647, "potato", 1 },
                    { 3, 3, 1, "wine", 1 },
                    { 4, 4, 1, "cake", 1 },
                    { 5, 1, 1, "egg", 0 },
                    { 6, 2, 1, "toast", 0 },
                    { 7, 3, 2147483647, "coffee", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dishes");
        }
    }
}
