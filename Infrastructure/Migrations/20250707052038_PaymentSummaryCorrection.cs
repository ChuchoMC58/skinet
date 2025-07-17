using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PaymentSummaryCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "shippingAddress_State",
                table: "Orders",
                newName: "ShippingAddress_State");

            migrationBuilder.RenameColumn(
                name: "shippingAddress_PostalCode",
                table: "Orders",
                newName: "ShippingAddress_PostalCode");

            migrationBuilder.RenameColumn(
                name: "shippingAddress_Name",
                table: "Orders",
                newName: "ShippingAddress_Name");

            migrationBuilder.RenameColumn(
                name: "shippingAddress_Line2",
                table: "Orders",
                newName: "ShippingAddress_Line2");

            migrationBuilder.RenameColumn(
                name: "shippingAddress_Line1",
                table: "Orders",
                newName: "ShippingAddress_Line1");

            migrationBuilder.RenameColumn(
                name: "shippingAddress_Country",
                table: "Orders",
                newName: "ShippingAddress_Country");

            migrationBuilder.RenameColumn(
                name: "shippingAddress_City",
                table: "Orders",
                newName: "ShippingAddress_City");

            migrationBuilder.RenameColumn(
                name: "paymentSummary_Year",
                table: "Orders",
                newName: "paymentSummary_ExpYear");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShippingAddress_State",
                table: "Orders",
                newName: "shippingAddress_State");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_PostalCode",
                table: "Orders",
                newName: "shippingAddress_PostalCode");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_Name",
                table: "Orders",
                newName: "shippingAddress_Name");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_Line2",
                table: "Orders",
                newName: "shippingAddress_Line2");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_Line1",
                table: "Orders",
                newName: "shippingAddress_Line1");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_Country",
                table: "Orders",
                newName: "shippingAddress_Country");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_City",
                table: "Orders",
                newName: "shippingAddress_City");

            migrationBuilder.RenameColumn(
                name: "paymentSummary_ExpYear",
                table: "Orders",
                newName: "paymentSummary_Year");
        }
    }
}
