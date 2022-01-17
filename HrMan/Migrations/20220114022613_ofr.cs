using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HrMan.Migrations
{
    public partial class ofr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginCount = table.Column<int>(nullable: false),
                    OverTime = table.Column<int>(nullable: false),
                    LoginDate = table.Column<DateTime>(nullable: false),
                    LogOffDate = table.Column<DateTime>(nullable: false),
                    IsLogin = table.Column<bool>(nullable: false),
                    IsLogOff = table.Column<bool>(nullable: false),
                    EmployeeId = table.Column<string>(nullable: true),
                    Late = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_EmployeeId",
                table: "Attendances",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");
        }
    }
}
