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
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    MaxCapacity = table.Column<int>(type: "int", nullable: false),
                    CurrentCapacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.StationId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    BankNum = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChosenPaymentCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Payed = table.Column<double>(type: "float", nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    P_MoMo = table.Column<bool>(type: "bit", nullable: true),
                    P_VNPay = table.Column<bool>(type: "bit", nullable: true),
                    P_ApplePay = table.Column<bool>(type: "bit", nullable: true),
                    P_PayPal = table.Column<bool>(type: "bit", nullable: true),
                    Passport = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.BankNum);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FarePerMin = table.Column<double>(type: "float", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    CurrentPos = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Code);
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
                    RentalStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StationId = table.Column<int>(type: "int", nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EncryptedPassport = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VehicleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                        principalColumn: "BankNum",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rentals_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Stations",
                columns: new[] { "StationId", "CurrentCapacity", "MaxCapacity", "Name" },
                values: new object[,]
                {
                    { 1, 3, 12, "Ben Thanh Market" },
                    { 2, 3, 10, "District 1 Hub" },
                    { 3, 2, 5, "Saigon Center" }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Code", "CurrentPos", "FarePerMin", "State", "Type" },
                values: new object[,]
                {
                    { "EB001", "Ben Thanh Market", 1000.0, 0, "EBike" },
                    { "EB002", "District 1 Hub", 1000.0, 0, "EBike" },
                    { "ES001", "Saigon Center", 1500.0, 0, "Scooter" },
                    { "ES002", "District 1 Hub", 1500.0, 0, "Scooter" },
                    { "ES003", "Saigon Center", 1500.0, 0, "Scooter" },
                    { "ES004", "District 1 Hub", 1500.0, 0, "Scooter" },
                    { "SB001", "Ben Thanh Market", 500.0, 0, "StandardBike" },
                    { "SB002", "Ben Thanh Market", 500.0, 0, "StandardBike" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "BankNum", "ChosenPaymentCode", "Payed", "UserType", "P_ApplePay", "P_PayPal", "Passport" },
                values: new object[,]
                {
                    { "INTL001", "applepay", 75000.0, "ForeignTourist", true, false, "A12345678" },
                    { "INTL002", "paypal", 150000.0, "ForeignTourist", false, true, "B98765432" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "BankNum", "ChosenPaymentCode", "Payed", "UserType", "P_MoMo", "P_VNPay" },
                values: new object[,]
                {
                    { "0123456789", "momo", 50000.0, "LocalCommuter", true, false },
                    { "0987654321", "vnpay", 100000.0, "LocalCommuter", true, true }
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
