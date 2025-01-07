using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PromoCodeFactory.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RestoredPartnerManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PartnerManagerId",
                table: "PromoCodes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodes_PartnerManagerId",
                table: "PromoCodes",
                column: "PartnerManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Employees_PartnerManagerId",
                table: "PromoCodes",
                column: "PartnerManagerId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Employees_PartnerManagerId",
                table: "PromoCodes");

            migrationBuilder.DropIndex(
                name: "IX_PromoCodes_PartnerManagerId",
                table: "PromoCodes");

            migrationBuilder.DropColumn(
                name: "PartnerManagerId",
                table: "PromoCodes");
        }
    }
}
