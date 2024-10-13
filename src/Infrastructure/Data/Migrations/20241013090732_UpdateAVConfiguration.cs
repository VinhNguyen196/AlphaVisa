using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlphaVisa.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAVConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressMap",
                table: "AVConfigurations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressMap",
                table: "AVConfigurations");
        }
    }
}
