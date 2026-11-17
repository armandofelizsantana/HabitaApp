using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Habita.Infrastructure.Identity.Migrations
{
                      
    public partial class AddAuditFieldsToAppUser : Migration
    {
                          
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                schema: "Identity",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                schema: "Identity",
                table: "Users",
                type: "datetimeoffset",
                nullable: true);
        }

                          
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "Identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "Identity",
                table: "Users");
        }
    }
}
