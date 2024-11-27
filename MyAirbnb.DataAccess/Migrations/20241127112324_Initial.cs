using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyAirbnb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accommodations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LocationTagName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    PricePerNight = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxAdultsAllowed = table.Column<int>(type: "integer", nullable: false),
                    MaxChildrenAllowed = table.Column<int>(type: "integer", nullable: false),
                    MaxInfantsAllowed = table.Column<int>(type: "integer", nullable: false),
                    MaxPetsAllowed = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accommodations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccommodationId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CheckInDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CheckOutDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalPrice = table.Column<double>(type: "double precision", nullable: false),
                    TotalAdults = table.Column<int>(type: "integer", nullable: false),
                    TotalChildren = table.Column<int>(type: "integer", nullable: false),
                    TotalInfants = table.Column<int>(type: "integer", nullable: false),
                    TotalPets = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Accommodations_AccommodationId",
                        column: x => x.AccommodationId,
                        principalTable: "Accommodations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Accommodations",
                columns: new[] { "Id", "Description", "LocationTagName", "MaxAdultsAllowed", "MaxChildrenAllowed", "MaxInfantsAllowed", "MaxPetsAllowed", "Name", "PricePerNight" },
                values: new object[,]
                {
                    { 1, "A beautiful beach house with stunning ocean views.", "Beach", 4, 2, 1, 1, "Beach House", 200.00m },
                    { 2, "A cozy cabin in the mountains, perfect for a getaway.", "Mountain", 2, 2, 1, 0, "Mountain Cabin", 150.00m },
                    { 3, "A modern apartment in the heart of the city.", "City", 2, 1, 1, 0, "City Apartment", 100.00m },
                    { 4, "A charming cottage in the countryside.", "Country", 3, 2, 1, 1, "Country Cottage", 120.00m },
                    { 5, "A serene house by the lake.", "Lake", 4, 3, 2, 1, "Lake House", 180.00m },
                    { 6, "A luxurious villa in the desert.", "Desert", 5, 3, 2, 0, "Desert Villa", 250.00m },
                    { 7, "A rustic lodge in the forest.", "Forest", 3, 2, 1, 1, "Forest Lodge", 160.00m },
                    { 8, "A private bungalow on a tropical island.", "Island", 2, 1, 1, 0, "Island Bungalow", 300.00m },
                    { 9, "A cozy chalet near the ski slopes.", "Ski", 4, 2, 1, 0, "Ski Chalet", 220.00m },
                    { 10, "A secluded retreat in the jungle.", "Jungle", 3, 2, 1, 1, "Jungle Retreat", 280.00m }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Password", "UserName" },
                values: new object[] { 1, "admin", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_AccommodationId",
                table: "Reservations",
                column: "AccommodationId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Accommodations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
