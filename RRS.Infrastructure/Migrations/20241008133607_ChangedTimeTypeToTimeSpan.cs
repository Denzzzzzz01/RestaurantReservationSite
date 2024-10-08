using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace RRS.Infrastructure.Migrations
{
    public partial class ChangedTimeTypeToTimeSpan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpeningHour",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ClosingHour",
                table: "Restaurants");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "OpeningHour",
                table: "Restaurants",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(9, 0, 0)); // Default opening hour

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ClosingHour",
                table: "Restaurants",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(18, 0, 0)); // Default closing hour

            // Upsert to avoid duplication
            migrationBuilder.Sql(@"
                DO 
                $$
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM ""AspNetRoles"" WHERE ""Name"" = 'Admin') THEN
                        INSERT INTO ""AspNetRoles"" (""Id"", ""ConcurrencyStamp"", ""Name"", ""NormalizedName"")
                        VALUES ('90ec6758-a373-4c67-9b6b-32cb1c062261', NULL, 'Admin', 'ADMIN');
                    END IF;

                    IF NOT EXISTS (SELECT 1 FROM ""AspNetRoles"" WHERE ""Name"" = 'User') THEN
                        INSERT INTO ""AspNetRoles"" (""Id"", ""ConcurrencyStamp"", ""Name"", ""NormalizedName"")
                        VALUES ('922fa616-e6c7-4d20-9654-bba28cefe3f5', NULL, 'User', 'USER');
                    END IF;
                END
                $$;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpeningHour",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ClosingHour",
                table: "Restaurants");

            migrationBuilder.AddColumn<int>(
                name: "OpeningHour",
                table: "Restaurants",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClosingHour",
                table: "Restaurants",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            // Removing roles
            migrationBuilder.Sql(@"
                DELETE FROM ""AspNetRoles"" WHERE ""Id"" = '90ec6758-a373-4c67-9b6b-32cb1c062261';
                DELETE FROM ""AspNetRoles"" WHERE ""Id"" = '922fa616-e6c7-4d20-9654-bba28cefe3f5';
            ");
        }
    }
}
