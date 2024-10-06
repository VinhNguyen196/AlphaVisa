using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlphaVisa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAttachmentItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Created",
                table: "TodoLists",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "TodoItems",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "ServiceItems",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "NewItems",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "ContactItems",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "AttachmentItems",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<int>(
                name: "Source",
                table: "AttachmentItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "AttachmentItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "AttachmentItems");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "AttachmentItems");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "TodoLists",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "TodoItems",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ServiceItems",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "NewItems",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ContactItems",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "AttachmentItems",
                newName: "Created");
        }
    }
}
