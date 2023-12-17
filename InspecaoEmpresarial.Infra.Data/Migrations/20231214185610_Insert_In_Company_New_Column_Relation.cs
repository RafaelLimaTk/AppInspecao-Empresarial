using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InspecaoEmpresarial.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class Insert_In_Company_New_Column_Relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Introduction",
                table: "Company",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Objective",
                table: "Company",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Introduction",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Objective",
                table: "Company");
        }
    }
}
