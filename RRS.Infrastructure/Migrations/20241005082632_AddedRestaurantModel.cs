using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RRS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedRestaurantModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("af8a8515-eada-419e-80c3-4f6f96de6d41"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e906baa4-d7f1-426f-b27f-3c280796e24e"));

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address_Street = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Address_City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address_Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address_State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Address_Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Address_Longitude = table.Column<double>(type: "double precision", nullable: false),
                    OpeningHour = table.Column<int>(type: "integer", nullable: false),
                    ClosingHour = table.Column<int>(type: "integer", nullable: false),
                    SeatingCapacity = table.Column<int>(type: "integer", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Website = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantManagerDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RestaurantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantManagerDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestaurantManagerDatas_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RestaurantManagerDatas_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("27ac20cc-c9ac-4816-8b3f-f5be4326efae"), null, "Admin", "ADMIN" },
                    { new Guid("59cafc34-fb88-4440-82b7-6cb7537ea9e2"), null, "User", "USER" },
                    { new Guid("7d8e4dc2-6315-4a79-acc6-e6ac8d13fe83"), null, "RestaurantManager", "RESTAURANTMANAGER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantManagerDatas_AppUserId",
                table: "RestaurantManagerDatas",
                column: "AppUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantManagerDatas_RestaurantId",
                table: "RestaurantManagerDatas",
                column: "RestaurantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RestaurantManagerDatas");

            migrationBuilder.DropTable(
                name: "Restaurants");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("27ac20cc-c9ac-4816-8b3f-f5be4326efae"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("59cafc34-fb88-4440-82b7-6cb7537ea9e2"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7d8e4dc2-6315-4a79-acc6-e6ac8d13fe83"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("af8a8515-eada-419e-80c3-4f6f96de6d41"), null, "User", "USER" },
                    { new Guid("e906baa4-d7f1-426f-b27f-3c280796e24e"), null, "Admin", "ADMIN" }
                });
        }
    }
}
