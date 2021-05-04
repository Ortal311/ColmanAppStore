using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ColmanAppStore.Migrations
{
    public partial class updateModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apps_Review_ReviewId",
                table: "Apps");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Apps");

            migrationBuilder.RenameColumn(
                name: "ReviewId",
                table: "Apps",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Raiting",
                table: "Apps",
                newName: "AverageRaiting");

            migrationBuilder.RenameIndex(
                name: "IX_Apps_ReviewId",
                table: "Apps",
                newName: "IX_Apps_UserId");

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserType",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Review",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "AppId",
                table: "Review",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Category",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Apps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeveloperName",
                table: "Apps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "countReview",
                table: "Apps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AppVideo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Video = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppVideo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppVideo_Apps_AppId",
                        column: x => x.AppId,
                        principalTable: "Apps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logo_Apps_AppsId",
                        column: x => x.AppsId,
                        principalTable: "Apps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardNumber = table.Column<long>(type: "bigint", nullable: false),
                    ExpiredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CVV = table.Column<int>(type: "int", maxLength: 4, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdNumber = table.Column<long>(type: "bigint", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_PaymentId",
                table: "User",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_AppId",
                table: "Review",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVideo_AppId",
                table: "AppVideo",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_Logo_AppsId",
                table: "Logo",
                column: "AppsId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Apps_User_UserId",
                table: "Apps",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Apps_AppId",
                table: "Review",
                column: "AppId",
                principalTable: "Apps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Payment_PaymentId",
                table: "User",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apps_User_UserId",
                table: "Apps");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Apps_AppId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Payment_PaymentId",
                table: "User");

            migrationBuilder.DropTable(
                name: "AppVideo");

            migrationBuilder.DropTable(
                name: "Logo");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_User_PaymentId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Review_AppId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "User");

            migrationBuilder.DropColumn(
                name: "AppId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "countReview",
                table: "Apps");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Apps",
                newName: "ReviewId");

            migrationBuilder.RenameColumn(
                name: "AverageRaiting",
                table: "Apps",
                newName: "Raiting");

            migrationBuilder.RenameIndex(
                name: "IX_Apps_UserId",
                table: "Apps",
                newName: "IX_Apps_ReviewId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Review",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Category",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Apps",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DeveloperName",
                table: "Apps",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Apps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Apps_Review_ReviewId",
                table: "Apps",
                column: "ReviewId",
                principalTable: "Review",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
