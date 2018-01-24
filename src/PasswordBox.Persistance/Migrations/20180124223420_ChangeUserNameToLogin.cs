using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PasswordBox.Persistance.Migrations
{
    public partial class ChangeUserNameToLogin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Vaults");

            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Vaults",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Login",
                table: "Vaults");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Vaults",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");
        }
    }
}
