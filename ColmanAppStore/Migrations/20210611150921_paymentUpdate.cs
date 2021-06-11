using Microsoft.EntityFrameworkCore.Migrations;

namespace ColmanAppStore.Migrations
{
    public partial class paymentUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentMethod_PaymentId",
                table: "PaymentMethod");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_PaymentId",
                table: "PaymentMethod",
                column: "PaymentId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentMethod_PaymentId",
                table: "PaymentMethod");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_PaymentId",
                table: "PaymentMethod",
                column: "PaymentId");
        }
    }
}
