using Microsoft.EntityFrameworkCore.Migrations;

namespace AMMCrawler.DAL.Migrations
{
    public partial class Inital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResourceLinks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    URL = table.Column<string>(type: "TEXT", nullable: true),
                    IsCrawled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Type = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceLinks", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ResourceMappings",
                columns: table => new
                {
                    CrawledLinkID = table.Column<int>(type: "INTEGER", nullable: false),
                    FoundLinkID = table.Column<int>(type: "INTEGER", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceMappings", x => new { x.FoundLinkID, x.CrawledLinkID });
                    table.ForeignKey(
                        name: "FK_ResourceMappings_ResourceLinks_CrawledLinkID",
                        column: x => x.CrawledLinkID,
                        principalTable: "ResourceLinks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResourceMappings_ResourceLinks_FoundLinkID",
                        column: x => x.FoundLinkID,
                        principalTable: "ResourceLinks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourceMappings_CrawledLinkID",
                table: "ResourceMappings",
                column: "CrawledLinkID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourceMappings");

            migrationBuilder.DropTable(
                name: "ResourceLinks");
        }
    }
}
