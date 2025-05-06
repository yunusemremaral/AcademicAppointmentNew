using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademicAppointmentApi.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_username_change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserFullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserFullName",
                table: "AspNetUsers");
        }
    }
}
