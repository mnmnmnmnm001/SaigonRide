using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SaigonRide.Migrations
{
    /// <inheritdoc />
    public partial class AddVehiclesToMatchCapacity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add more vehicles to reach 125 total (matching station capacity: 12 + 10 + 5 = 27)
            // Actually the seed data sets capacities to 3, 3, 2 = 8 total, but we need 125 to match intended capacity
            // Let's add 117 more vehicles (SB003-SB045 = 43, EB003-EB044 = 42, ES005-ES036 = 32)
            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Code", "CurrentPos", "FarePerMin", "State", "Type" },
                values: new object[,]
                {
                    // StandardBikes SB003-SB045 (43 total)
                    { "SB003", "Ben Thanh Market", 500.0, 0, "StandardBike" },
                    { "SB004", "Ben Thanh Market", 500.0, 0, "StandardBike" },
                    { "SB005", "Ben Thanh Market", 500.0, 0, "StandardBike" },
                    { "SB006", "District 1 Hub", 500.0, 0, "StandardBike" },
                    { "SB007", "District 1 Hub", 500.0, 0, "StandardBike" },
                    { "SB008", "District 1 Hub", 500.0, 0, "StandardBike" },
                    { "SB009", "Saigon Center", 500.0, 0, "StandardBike" },
                    { "SB010", "Saigon Center", 500.0, 0, "StandardBike" },
                    { "SB011", "Saigon Center", 500.0, 0, "StandardBike" },
                    { "SB012", "Ben Thanh Market", 500.0, 0, "StandardBike" },
                    { "SB013", "District 1 Hub", 500.0, 0, "StandardBike" },
                    { "SB014", "Saigon Center", 500.0, 0, "StandardBike" },
                    { "SB015", "Ben Thanh Market", 500.0, 0, "StandardBike" },
                    { "SB016", "District 1 Hub", 500.0, 0, "StandardBike" },
                    { "SB017", "Saigon Center", 500.0, 0, "StandardBike" },
                    { "SB018", "Ben Thanh Market", 500.0, 0, "StandardBike" },
                    { "SB019", "District 1 Hub", 500.0, 0, "StandardBike" },
                    { "SB020", "Saigon Center", 500.0, 0, "StandardBike" },
                    { "SB021", "Ben Thanh Market", 500.0, 0, "StandardBike" },
                    { "SB022", "District 1 Hub", 500.0, 0, "StandardBike" },
                    { "SB023", "Saigon Center", 500.0, 0, "StandardBike" },
                    { "SB024", "Ben Thanh Market", 500.0, 0, "StandardBike" },
                    { "SB025", "District 1 Hub", 500.0, 0, "StandardBike" },
                    { "SB026", "Saigon Center", 500.0, 0, "StandardBike" },
                    { "SB027", "Ben Thanh Market", 500.0, 0, "StandardBike" },
                    { "SB028", "District 1 Hub", 500.0, 0, "StandardBike" },
                    { "SB029", "Saigon Center", 500.0, 0, "StandardBike" },
                    { "SB030", "Ben Thanh Market", 500.0, 0, "StandardBike" },
                    { "SB031", "District 1 Hub", 500.0, 0, "StandardBike" },
                    { "SB032", "Saigon Center", 500.0, 0, "StandardBike" },
                    { "SB033", "Ben Thanh Market", 500.0, 0, "StandardBike" },
                    { "SB034", "District 1 Hub", 500.0, 0, "StandardBike" },
                    { "SB035", "Saigon Center", 500.0, 0, "StandardBike" },
                    { "SB036", "Ben Thanh Market", 500.0, 0, "StandardBike" },
                    { "SB037", "District 1 Hub", 500.0, 0, "StandardBike" },
                    { "SB038", "Saigon Center", 500.0, 0, "StandardBike" },
                    { "SB039", "Ben Thanh Market", 500.0, 0, "StandardBike" },
                    { "SB040", "District 1 Hub", 500.0, 0, "StandardBike" },
                    { "SB041", "Saigon Center", 500.0, 0, "StandardBike" },
                    { "SB042", "Ben Thanh Market", 500.0, 0, "StandardBike" },
                    { "SB043", "District 1 Hub", 500.0, 0, "StandardBike" },
                    { "SB044", "Saigon Center", 500.0, 0, "StandardBike" },
                    { "SB045", "Ben Thanh Market", 500.0, 0, "StandardBike" },

                    // EBikes EB003-EB044 (42 total)
                    { "EB003", "Ben Thanh Market", 1000.0, 0, "EBike" },
                    { "EB004", "District 1 Hub", 1000.0, 0, "EBike" },
                    { "EB005", "Saigon Center", 1000.0, 0, "EBike" },
                    { "EB006", "Ben Thanh Market", 1000.0, 0, "EBike" },
                    { "EB007", "District 1 Hub", 1000.0, 0, "EBike" },
                    { "EB008", "Saigon Center", 1000.0, 0, "EBike" },
                    { "EB009", "Ben Thanh Market", 1000.0, 0, "EBike" },
                    { "EB010", "District 1 Hub", 1000.0, 0, "EBike" },
                    { "EB011", "Saigon Center", 1000.0, 0, "EBike" },
                    { "EB012", "Ben Thanh Market", 1000.0, 0, "EBike" },
                    { "EB013", "District 1 Hub", 1000.0, 0, "EBike" },
                    { "EB014", "Saigon Center", 1000.0, 0, "EBike" },
                    { "EB015", "Ben Thanh Market", 1000.0, 0, "EBike" },
                    { "EB016", "District 1 Hub", 1000.0, 0, "EBike" },
                    { "EB017", "Saigon Center", 1000.0, 0, "EBike" },
                    { "EB018", "Ben Thanh Market", 1000.0, 0, "EBike" },
                    { "EB019", "District 1 Hub", 1000.0, 0, "EBike" },
                    { "EB020", "Saigon Center", 1000.0, 0, "EBike" },
                    { "EB021", "Ben Thanh Market", 1000.0, 0, "EBike" },
                    { "EB022", "District 1 Hub", 1000.0, 0, "EBike" },
                    { "EB023", "Saigon Center", 1000.0, 0, "EBike" },
                    { "EB024", "Ben Thanh Market", 1000.0, 0, "EBike" },
                    { "EB025", "District 1 Hub", 1000.0, 0, "EBike" },
                    { "EB026", "Saigon Center", 1000.0, 0, "EBike" },
                    { "EB027", "Ben Thanh Market", 1000.0, 0, "EBike" },
                    { "EB028", "District 1 Hub", 1000.0, 0, "EBike" },
                    { "EB029", "Saigon Center", 1000.0, 0, "EBike" },
                    { "EB030", "Ben Thanh Market", 1000.0, 0, "EBike" },
                    { "EB031", "District 1 Hub", 1000.0, 0, "EBike" },
                    { "EB032", "Saigon Center", 1000.0, 0, "EBike" },
                    { "EB033", "Ben Thanh Market", 1000.0, 0, "EBike" },
                    { "EB034", "District 1 Hub", 1000.0, 0, "EBike" },
                    { "EB035", "Saigon Center", 1000.0, 0, "EBike" },
                    { "EB036", "Ben Thanh Market", 1000.0, 0, "EBike" },
                    { "EB037", "District 1 Hub", 1000.0, 0, "EBike" },
                    { "EB038", "Saigon Center", 1000.0, 0, "EBike" },
                    { "EB039", "Ben Thanh Market", 1000.0, 0, "EBike" },
                    { "EB040", "District 1 Hub", 1000.0, 0, "EBike" },
                    { "EB041", "Saigon Center", 1000.0, 0, "EBike" },
                    { "EB042", "Ben Thanh Market", 1000.0, 0, "EBike" },
                    { "EB043", "District 1 Hub", 1000.0, 0, "EBike" },
                    { "EB044", "Saigon Center", 1000.0, 0, "EBike" },

                    // Scooters ES005-ES036 (32 total)
                    { "ES005", "Ben Thanh Market", 1500.0, 0, "Scooter" },
                    { "ES006", "District 1 Hub", 1500.0, 0, "Scooter" },
                    { "ES007", "Saigon Center", 1500.0, 0, "Scooter" },
                    { "ES008", "Ben Thanh Market", 1500.0, 0, "Scooter" },
                    { "ES009", "District 1 Hub", 1500.0, 0, "Scooter" },
                    { "ES010", "Saigon Center", 1500.0, 0, "Scooter" },
                    { "ES011", "Ben Thanh Market", 1500.0, 0, "Scooter" },
                    { "ES012", "District 1 Hub", 1500.0, 0, "Scooter" },
                    { "ES013", "Saigon Center", 1500.0, 0, "Scooter" },
                    { "ES014", "Ben Thanh Market", 1500.0, 0, "Scooter" },
                    { "ES015", "District 1 Hub", 1500.0, 0, "Scooter" },
                    { "ES016", "Saigon Center", 1500.0, 0, "Scooter" },
                    { "ES017", "Ben Thanh Market", 1500.0, 0, "Scooter" },
                    { "ES018", "District 1 Hub", 1500.0, 0, "Scooter" },
                    { "ES019", "Saigon Center", 1500.0, 0, "Scooter" },
                    { "ES020", "Ben Thanh Market", 1500.0, 0, "Scooter" },
                    { "ES021", "District 1 Hub", 1500.0, 0, "Scooter" },
                    { "ES022", "Saigon Center", 1500.0, 0, "Scooter" },
                    { "ES023", "Ben Thanh Market", 1500.0, 0, "Scooter" },
                    { "ES024", "District 1 Hub", 1500.0, 0, "Scooter" },
                    { "ES025", "Saigon Center", 1500.0, 0, "Scooter" },
                    { "ES026", "Ben Thanh Market", 1500.0, 0, "Scooter" },
                    { "ES027", "District 1 Hub", 1500.0, 0, "Scooter" },
                    { "ES028", "Saigon Center", 1500.0, 0, "Scooter" },
                    { "ES029", "Ben Thanh Market", 1500.0, 0, "Scooter" },
                    { "ES030", "District 1 Hub", 1500.0, 0, "Scooter" },
                    { "ES031", "Saigon Center", 1500.0, 0, "Scooter" },
                    { "ES032", "Ben Thanh Market", 1500.0, 0, "Scooter" },
                    { "ES033", "District 1 Hub", 1500.0, 0, "Scooter" },
                    { "ES034", "Saigon Center", 1500.0, 0, "Scooter" },
                    { "ES035", "Ben Thanh Market", 1500.0, 0, "Scooter" },
                    { "ES036", "District 1 Hub", 1500.0, 0, "Scooter" }
                });

            // Update station capacities to match the total number of vehicles (125)
            migrationBuilder.UpdateData(
                table: "Stations",
                keyColumn: "StationId",
                keyValue: 1,
                columns: new[] { "CurrentCapacity", "MaxCapacity" },
                values: new object[] { 110, 110 });

            migrationBuilder.UpdateData(
                table: "Stations",
                keyColumn: "StationId",
                keyValue: 2,
                columns: new[] { "CurrentCapacity", "MaxCapacity" },
                values: new object[] { 10, 10 });

            migrationBuilder.UpdateData(
                table: "Stations",
                keyColumn: "StationId",
                keyValue: 3,
                columns: new[] { "CurrentCapacity", "MaxCapacity" },
                values: new object[] { 5, 5 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete all the added vehicles
            for (int i = 3; i <= 45; i++)
            {
                migrationBuilder.DeleteData(
                    table: "Vehicles",
                    keyColumn: "Code",
                    keyValue: $"SB{i:D3}");
            }

            for (int i = 3; i <= 44; i++)
            {
                migrationBuilder.DeleteData(
                    table: "Vehicles",
                    keyColumn: "Code",
                    keyValue: $"EB{i:D3}");
            }

            for (int i = 5; i <= 36; i++)
            {
                migrationBuilder.DeleteData(
                    table: "Vehicles",
                    keyColumn: "Code",
                    keyValue: $"ES{i:D3}");
            }

            // Revert station capacities
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
        }
    }
}
