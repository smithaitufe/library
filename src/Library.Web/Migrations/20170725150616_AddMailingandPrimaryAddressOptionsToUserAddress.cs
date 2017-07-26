using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Web.Migrations
{
    public partial class AddMailingandPrimaryAddressOptionsToUserAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Mailing",
                table: "UserAddresses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Primary",
                table: "UserAddresses",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mailing",
                table: "UserAddresses");

            migrationBuilder.DropColumn(
                name: "Primary",
                table: "UserAddresses");
        }
    }
}
