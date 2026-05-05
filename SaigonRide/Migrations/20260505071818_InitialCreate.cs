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
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Users_UserId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Vehicles_VehicleId",
                table: "Rentals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "StationId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyColumnType: "int",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyColumnType: "int",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyColumnType: "int",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyColumnType: "int",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "VehicleId",
                keyColumnType: "int",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "VehicleId",
                keyColumnType: "int",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "VehicleId",
                keyColumnType: "int",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "VehicleId",
                keyColumnType: "int",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "VehicleId",
                keyColumnType: "int",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "VehicleId",
                keyColumnType: "int",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "Ratio",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Admins");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Vehicles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BankNum",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "VehicleId",
                table: "Rentals",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Rentals",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles",
                column: "Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "BankNum");

            migrationBuilder.UpdateData(
                table: "Stations",
                keyColumn: "StationId",
                keyValue: 1,
                columns: new[] { "CurrentCapacity", "MaxCapacity" },
                values: new object[] { 3, 12 });

            migrationBuilder.UpdateData(
                table: "Stations",
                keyColumn: "StationId",
                keyValue: 2,
                columns: new[] { "CurrentCapacity", "MaxCapacity" },
                values: new object[] { 3, 10 });

            migrationBuilder.UpdateData(
                table: "Stations",
                keyColumn: "StationId",
                keyValue: 3,
                columns: new[] { "CurrentCapacity", "MaxCapacity" },
                values: new object[] { 2, 5 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "BankNum", "ChosenPaymentCode", "P_MoMo", "P_VNPay", "Payed", "UserType" },
                values: new object[,]
                {
                    { "0123456789", "momo", true, false, 50000.0, "LocalCommuter" },
                    { "0987654321", "vnpay", true, true, 100000.0, "LocalCommuter" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "BankNum", "ChosenPaymentCode", "P_ApplePay", "P_PayPal", "Passport", "Payed", "UserType" },
                values: new object[,]
                {
                    { "INTL001", "applepay", true, false, "A12345678", 75000.0, "ForeignTourist" },
                    { "INTL002", "paypal", false, true, "B98765432", 150000.0, "ForeignTourist" }
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

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Users_UserId",
                table: "Rentals",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "BankNum",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Vehicles_VehicleId",
                table: "Rentals",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Users_UserId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Vehicles_VehicleId",
                table: "Rentals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "BankNum",
                keyValue: "0123456789");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "BankNum",
                keyValue: "0987654321");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "BankNum",
                keyValue: "INTL001");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "BankNum",
                keyValue: "INTL002");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Code",
                keyValue: "EB001");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Code",
                keyValue: "EB002");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Code",
                keyValue: "ES001");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Code",
                keyValue: "ES002");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Code",
                keyValue: "ES003");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Code",
                keyValue: "ES004");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Code",
                keyValue: "SB001");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Code",
                keyValue: "SB002");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "BankNum",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Stations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Stations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Ratio",
                table: "Stations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "VehicleId",
                table: "Rentals",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Rentals",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Admins",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles",
                column: "VehicleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");

            migrationBuilder.UpdateData(
                table: "Stations",
                keyColumn: "StationId",
                keyValue: 1,
                columns: new[] { "CurrentCapacity", "Latitude", "Longitude", "MaxCapacity", "Ratio" },
                values: new object[] { 15, 10.772500000000001, 106.6992, 20, 0.75 });

            migrationBuilder.UpdateData(
                table: "Stations",
                keyColumn: "StationId",
                keyValue: 2,
                columns: new[] { "CurrentCapacity", "Latitude", "Longitude", "MaxCapacity", "Ratio" },
                values: new object[] { 8, 10.7758, 106.7008, 30, 0.27000000000000002 });

            migrationBuilder.UpdateData(
                table: "Stations",
                keyColumn: "StationId",
                keyValue: 3,
                columns: new[] { "CurrentCapacity", "Latitude", "Longitude", "MaxCapacity", "Ratio" },
                values: new object[] { 28, 10.776899999999999, 106.7009, 35, 0.80000000000000004 });

            migrationBuilder.InsertData(
                table: "Stations",
                columns: new[] { "StationId", "CurrentCapacity", "Latitude", "Longitude", "MaxCapacity", "Name", "Ratio" },
                values: new object[] { 4, 5, 10.807399999999999, 106.6756, 25, "Tan Binh Station", 0.20000000000000001 });

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

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Users_UserId",
                table: "Rentals",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Vehicles_VehicleId",
                table: "Rentals",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "VehicleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
