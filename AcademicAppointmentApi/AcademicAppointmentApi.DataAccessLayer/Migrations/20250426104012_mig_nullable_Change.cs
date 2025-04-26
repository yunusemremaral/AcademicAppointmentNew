using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademicAppointmentApi.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_nullable_Change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rooms_AppUserId",
                table: "Rooms");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Rooms",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_AppUserId",
                table: "Rooms",
                column: "AppUserId",
                unique: true,
                filter: "[AppUserId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rooms_AppUserId",
                table: "Rooms");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Rooms",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_AppUserId",
                table: "Rooms",
                column: "AppUserId",
                unique: true);
        }
    }
}
