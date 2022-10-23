using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthTrackingSystem.Infrastructure.Migrations
{
    public partial class PatientCaretackers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PatientCaretakerId",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PatientCaretakers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientCaretakers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientCaretakers_DomainUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "DomainUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientCaretakers_UserId",
                table: "PatientCaretakers",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_PatientCaretakers_DoctorId",
                table: "Patients",
                column: "DoctorId",
                principalTable: "PatientCaretakers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_PatientCaretakers_DoctorId",
                table: "Patients");

            migrationBuilder.DropTable(
                name: "PatientCaretakers");

            migrationBuilder.DropColumn(
                name: "PatientCaretakerId",
                table: "Patients");
        }
    }
}
