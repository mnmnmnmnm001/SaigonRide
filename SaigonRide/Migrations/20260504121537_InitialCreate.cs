using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SaigonRide.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdminId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    StationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentCapacity = table.Column<int>(type: "int", nullable: false),
                    MaxCapacity = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Ratio = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.StationId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChosenPaymentCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Payed = table.Column<double>(type: "float", nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    Passport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    P_ApplePay = table.Column<bool>(type: "bit", nullable: true),
                    P_PayPal = table.Column<bool>(type: "bit", nullable: true),
                    P_MoMo = table.Column<bool>(type: "bit", nullable: true),
                    P_VNPay = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehicleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FarePerMin = table.Column<double>(type: "float", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentPos = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleId);
                });

            migrationBuilder.CreateTable(
                name: "RentalTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EncryptedBankNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EncryptedVehicleCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinutesBorrowed = table.Column<int>(type: "int", nullable: false),
                    MoneyRented = table.Column<double>(type: "float", nullable: false),
                    EncryptedPassport = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RentalStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalTransactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_RentalTransactions_Stations_StationId",
                        column: x => x.StationId,
                        principalTable: "Stations",
                        principalColumn: "StationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rentals",
                columns: table => new
                {
                    RentalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    StartStationId = table.Column<int>(type: "int", nullable: false),
                    EndStationId = table.Column<int>(type: "int", nullable: false),
                    TimeStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CalculatedFare = table.Column<double>(type: "float", nullable: false),
                    DiscountApplied = table.Column<double>(type: "float", nullable: false),
                    FinalFare = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rentals", x => x.RentalId);
                    table.ForeignKey(
                        name: "FK_Rentals_Stations_EndStationId",
                        column: x => x.EndStationId,
                        principalTable: "Stations",
                        principalColumn: "StationId");
                    table.ForeignKey(
                        name: "FK_Rentals_Stations_StartStationId",
                        column: x => x.StartStationId,
                        principalTable: "Stations",
                        principalColumn: "StationId");
                    table.ForeignKey(
                        name: "FK_Rentals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rentals_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Stations",
                columns: new[] { "StationId", "CurrentCapacity", "Latitude", "Longitude", "MaxCapacity", "Name", "Ratio" },
                values: new object[,]
                {
                    { 1, 15, 10.772500000000001, 106.6992, 20, "Ben Thanh Market", 0.75 },
                    { 2, 8, 10.7758, 106.7008, 30, "District 1 Hub", 0.27000000000000002 },
                    { 3, 28, 10.776899999999999, 106.7009, 35, "Saigon Center", 0.80000000000000004 },
                    { 4, 5, 10.807399999999999, 106.6756, 25, "Tan Binh Station", 0.20000000000000001 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BankNum", "ChosenPaymentCode", "P_MoMo", "P_VNPay", "Payed", "UserType" },
                values: new object[,]
                {
                    { 1, "0123456789", "momo", true, false, 50000.0, "LocalCommuter" },
                    { 2, "0987654321", "vnpay", true, true, 100000.0, "LocalCommuter" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BankNum", "ChosenPaymentCode", "P_ApplePay", "P_PayPal", "Passport", "Payed", "UserType" },
                values: new object[,]
                {
                    { 3, "INTL001", "applepay", true, false, "A12345678", 75000.0, "ForeignTourist" },
                    { 4, "INTL002", "paypal", false, true, "B98765432", 150000.0, "ForeignTourist" }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "VehicleId", "Code", "CurrentPos", "FarePerMin", "State", "Type" },
                values: new object[,]
                {
                    { 1, "SB001", "Station 1", 500.0, 0, "StandardBike" },
                    { 2, "SB002", "Station 1", 500.0, 0, "StandardBike" },
                    { 3, "EB001", "Station 2", 1000.0, 0, "EBike" },
                    { 4, "EB002", "Station 2", 1000.0, 0, "EBike" },
                    { 5, "ES001", "Station 3", 1500.0, 0, "Scooter" },
                    { 6, "ES002", "Station 3", 1500.0, 0, "Scooter" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_EndStationId",
                table: "Rentals",
                column: "EndStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_StartStationId",
                table: "Rentals",
                column: "StartStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_UserId",
                table: "Rentals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_VehicleId",
                table: "Rentals",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalTransactions_StationId",
                table: "RentalTransactions",
                column: "StationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Rentals");

            migrationBuilder.DropTable(
                name: "RentalTransactions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Stations");
        }
    }
}
