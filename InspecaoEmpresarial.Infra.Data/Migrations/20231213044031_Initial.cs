using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InspecaoEmpresarial.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CorporateName = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    CNPJ = table.Column<string>(type: "TEXT", nullable: true),
                    CNAE = table.Column<string>(type: "TEXT", nullable: true),
                    RiskGrade = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Establishment",
                columns: table => new
                {
                    EstablishmentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Location = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    Phone = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Establishment", x => x.EstablishmentId);
                    table.ForeignKey(
                        name: "FK_Establishment_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessDescription",
                columns: table => new
                {
                    ProcessDescriptionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Department = table.Column<string>(type: "TEXT", nullable: true),
                    Activity = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessDescription", x => x.ProcessDescriptionId);
                    table.ForeignKey(
                        name: "FK_ProcessDescription_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Responsibility",
                columns: table => new
                {
                    ResponsibilityId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Superintendence = table.Column<string>(type: "TEXT", nullable: true),
                    Management = table.Column<string>(type: "TEXT", nullable: true),
                    InCharge = table.Column<string>(type: "TEXT", nullable: true),
                    SE = table.Column<string>(type: "TEXT", nullable: true),
                    SMT = table.Column<string>(type: "TEXT", nullable: true),
                    CIPA = table.Column<string>(type: "TEXT", nullable: true),
                    FireBrigade = table.Column<string>(type: "TEXT", nullable: true),
                    Employees = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responsibility", x => x.ResponsibilityId);
                    table.ForeignKey(
                        name: "FK_Responsibility_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Establishment_CompanyId",
                table: "Establishment",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDescription_CompanyId",
                table: "ProcessDescription",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Responsibility_CompanyId",
                table: "Responsibility",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Establishment");

            migrationBuilder.DropTable(
                name: "ProcessDescription");

            migrationBuilder.DropTable(
                name: "Responsibility");

            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
