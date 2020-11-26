using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace webApi.Migrations
{
    public partial class i : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("11000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2020, 6, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "G456", "Li", 1, "huahua" });

            migrationBuilder.InsertData(
                table: "employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("11000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2005, 8, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "G457", "xiao", 1, "ming" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "employees",
                keyColumn: "Id",
                keyValue: new Guid("11000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "employees",
                keyColumn: "Id",
                keyValue: new Guid("11000000-0000-0000-0000-000000000002"));
        }
    }
}
