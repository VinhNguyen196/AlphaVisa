using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlphaVisa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContactItemAndAddAVConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Metadata",
                table: "ContactItems",
                newName: "Content");

            migrationBuilder.CreateTable(
                name: "AVConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SocialLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AVConfigurations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AVConfigurations");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "ContactItems",
                newName: "Metadata");
        }
    }
}
