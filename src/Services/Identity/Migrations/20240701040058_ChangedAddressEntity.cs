using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Migrations
{
    /// <inheritdoc />
    public partial class ChangedAddressEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SavedUserAddresses",
                keyColumn: "Id",
                keyValue: new Guid("b07dd4ae-fc21-49e6-ade5-afcb50d8c010"));

            migrationBuilder.DeleteData(
                table: "SavedUserPaymentCards",
                keyColumn: "Id",
                keyValue: new Guid("3940d311-c8ab-4030-aef0-04fcb9da3cc4"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "CustomerId",
                keyValue: new Guid("5b7e1118-daa3-4467-b6dd-ed5199f3f070"));

            migrationBuilder.AddColumn<string>(
                name: "FullAddress",
                table: "SavedUserAddresses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "CustomerId", "Birthday", "Email", "Name", "Password", "PhoneNumber" },
                values: new object[] { new Guid("b3cf2665-45b8-4437-8e1f-8a29b8b734fa"), new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), "test@gmail.com", "Test User", "test", "+79012223344" });

            migrationBuilder.InsertData(
                table: "SavedUserAddresses",
                columns: new[] { "Id", "Apartment", "City", "CustomerId", "FullAddress", "House", "Street" },
                values: new object[] { new Guid("2d29819f-6ec7-4297-922a-36c6da163695"), "1", "Братск", new Guid("b3cf2665-45b8-4437-8e1f-8a29b8b734fa"), "Братск, Советская 1-1", "1", "Советская" });

            migrationBuilder.InsertData(
                table: "SavedUserPaymentCards",
                columns: new[] { "Id", "CardHolderName", "CardNumber", "CustomerId", "Cvv", "Expiration" },
                values: new object[] { new Guid("11cb7280-2194-419b-9104-8e820d38af9d"), "IVAN IVANOV", "1111 2222 3333 4444", new Guid("b3cf2665-45b8-4437-8e1f-8a29b8b734fa"), "111", "10/25" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SavedUserAddresses",
                keyColumn: "Id",
                keyValue: new Guid("2d29819f-6ec7-4297-922a-36c6da163695"));

            migrationBuilder.DeleteData(
                table: "SavedUserPaymentCards",
                keyColumn: "Id",
                keyValue: new Guid("11cb7280-2194-419b-9104-8e820d38af9d"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "CustomerId",
                keyValue: new Guid("b3cf2665-45b8-4437-8e1f-8a29b8b734fa"));

            migrationBuilder.DropColumn(
                name: "FullAddress",
                table: "SavedUserAddresses");

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
        }
    }
}
