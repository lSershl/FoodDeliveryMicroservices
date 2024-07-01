using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Migrations
{
    /// <inheritdoc />
    public partial class ChangedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "CustomerId",
                keyValue: new Guid("a05a70d2-6b85-4cea-91f5-3501cf827a7f"));

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "SavedUserAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: false),
                    House = table.Column<string>(type: "text", nullable: false),
                    Apartment = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedUserAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedUserAddresses_Users_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Users",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavedUserPaymentCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CardNumber = table.Column<string>(type: "text", nullable: false),
                    Cvv = table.Column<string>(type: "text", nullable: false),
                    Expiration = table.Column<string>(type: "text", nullable: false),
                    CardHolderName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedUserPaymentCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedUserPaymentCards_Users_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Users",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "CustomerId", "Birthday", "Email", "Name", "Password", "PhoneNumber" },
                values: new object[] { new Guid("5b7e1118-daa3-4467-b6dd-ed5199f3f070"), new DateTime(2024, 6, 30, 0, 0, 0, 0, DateTimeKind.Utc), "test@gmail.com", "Test User", "test", "+79012223344" });

            migrationBuilder.InsertData(
                table: "SavedUserAddresses",
                columns: new[] { "Id", "Apartment", "City", "CustomerId", "House", "Street" },
                values: new object[] { new Guid("b07dd4ae-fc21-49e6-ade5-afcb50d8c010"), "1", "Братск", new Guid("5b7e1118-daa3-4467-b6dd-ed5199f3f070"), "1", "Советская" });

            migrationBuilder.InsertData(
                table: "SavedUserPaymentCards",
                columns: new[] { "Id", "CardHolderName", "CardNumber", "CustomerId", "Cvv", "Expiration" },
                values: new object[] { new Guid("3940d311-c8ab-4030-aef0-04fcb9da3cc4"), "IVAN IVANOV", "1111 2222 3333 4444", new Guid("5b7e1118-daa3-4467-b6dd-ed5199f3f070"), "111", "10/25" });

            migrationBuilder.CreateIndex(
                name: "IX_SavedUserAddresses_CustomerId",
                table: "SavedUserAddresses",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedUserPaymentCards_CustomerId",
                table: "SavedUserPaymentCards",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedUserAddresses");

            migrationBuilder.DropTable(
                name: "SavedUserPaymentCards");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "CustomerId",
                keyValue: new Guid("5b7e1118-daa3-4467-b6dd-ed5199f3f070"));

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "CustomerId", "Address", "Birthday", "Email", "Name", "Password", "PhoneNumber" },
                values: new object[] { new Guid("a05a70d2-6b85-4cea-91f5-3501cf827a7f"), "Советская 1-1", new DateTime(2024, 6, 27, 0, 0, 0, 0, DateTimeKind.Utc), "test@gmail.com", "Test User", "test", "+79012223344" });
        }
    }
}
