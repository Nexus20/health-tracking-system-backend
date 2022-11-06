using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthTrackingSystem.Infrastructure.Migrations
{
    public partial class CaretakersConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_PatientCaretakers_DoctorId",
                table: "Patients");

            migrationBuilder.AlterColumn<string>(
                name: "PatientCaretakerId",
                table: "Patients",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PatientCaretakerId",
                table: "Patients",
                column: "PatientCaretakerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_PatientCaretakers_PatientCaretakerId",
                table: "Patients",
                column: "PatientCaretakerId",
                principalTable: "PatientCaretakers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_PatientCaretakers_PatientCaretakerId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_PatientCaretakerId",
                table: "Patients");

            migrationBuilder.AlterColumn<string>(
                name: "PatientCaretakerId",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_PatientCaretakers_DoctorId",
                table: "Patients",
                column: "DoctorId",
                principalTable: "PatientCaretakers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
