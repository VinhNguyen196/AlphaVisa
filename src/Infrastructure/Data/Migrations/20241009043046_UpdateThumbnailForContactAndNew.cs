using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlphaVisa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateThumbnailForContactAndNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "NewItems");

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "ContactItems");

            migrationBuilder.AddColumn<Guid>(
                name: "AttachmentItemId",
                table: "NewItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AttachmentItemId",
                table: "ContactItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewItems_AttachmentItemId",
                table: "NewItems",
                column: "AttachmentItemId",
                unique: true,
                filter: "[AttachmentItemId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ContactItems_AttachmentItemId",
                table: "ContactItems",
                column: "AttachmentItemId",
                unique: true,
                filter: "[AttachmentItemId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactItems_AttachmentItems_AttachmentItemId",
                table: "ContactItems",
                column: "AttachmentItemId",
                principalTable: "AttachmentItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NewItems_AttachmentItems_AttachmentItemId",
                table: "NewItems",
                column: "AttachmentItemId",
                principalTable: "AttachmentItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactItems_AttachmentItems_AttachmentItemId",
                table: "ContactItems");

            migrationBuilder.DropForeignKey(
                name: "FK_NewItems_AttachmentItems_AttachmentItemId",
                table: "NewItems");

            migrationBuilder.DropIndex(
                name: "IX_NewItems_AttachmentItemId",
                table: "NewItems");

            migrationBuilder.DropIndex(
                name: "IX_ContactItems_AttachmentItemId",
                table: "ContactItems");

            migrationBuilder.DropColumn(
                name: "AttachmentItemId",
                table: "NewItems");

            migrationBuilder.DropColumn(
                name: "AttachmentItemId",
                table: "ContactItems");

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "NewItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "ContactItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
