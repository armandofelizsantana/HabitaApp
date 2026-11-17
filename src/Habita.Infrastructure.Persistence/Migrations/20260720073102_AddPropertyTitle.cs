using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Habita.Infrastructure.Persistence.Migrations
{
                      
    public partial class AddPropertyTitle : Migration
    {
                          
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Properties",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "");
        }

                          
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Properties");
        }
    }
}
