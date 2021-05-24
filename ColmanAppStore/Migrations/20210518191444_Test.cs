using Microsoft.EntityFrameworkCore.Migrations;

namespace ColmanAppStore.Migrations
{
    public partial class Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Payment_PaymentId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_PaymentId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "User");

            migrationBuilder.AlterColumn<int>(
                name: "UserType",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentUser",
                columns: table => new
                {
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentUser", x => new { x.PaymentMethodId, x.UserId });
                    table.ForeignKey(
                        name: "FK_PaymentUser_Payment_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "Payment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentUser_UserId",
                table: "PaymentUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentUser");

            migrationBuilder.AlterColumn<string>(
                name: "UserType",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_PaymentId",
                table: "User",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Payment_PaymentId",
                table: "User",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
