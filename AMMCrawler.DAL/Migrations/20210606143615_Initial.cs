using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AMMCrawler.DAL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    RootURL = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ResourceLinks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    URL = table.Column<string>(type: "TEXT", nullable: true),
                    IsCrawled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Type = table.Column<byte>(type: "INTEGER", nullable: false),
                    ApplicationID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceLinks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ResourceLinks_Applications_ApplicationID",
                        column: x => x.ApplicationID,
                        principalTable: "Applications",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RunInfo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Start = table.Column<DateTime>(type: "TEXT", nullable: false),
                    End = table.Column<DateTime>(type: "TEXT", nullable: false),
                    InnerCount = table.Column<int>(type: "INTEGER", nullable: false),
                    OuterCount = table.Column<int>(type: "INTEGER", nullable: false),
                    EtcCount = table.Column<int>(type: "INTEGER", nullable: false),
                    ApplicationID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunInfo", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RunInfo_Applications_ApplicationID",
                        column: x => x.ApplicationID,
                        principalTable: "Applications",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ETCLinksMetadata",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsInternal = table.Column<bool>(type: "INTEGER", nullable: false),
                    Type = table.Column<byte>(type: "INTEGER", nullable: false),
                    ResourceLinkID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETCLinksMetadata", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ETCLinksMetadata_ResourceLinks_ResourceLinkID",
                        column: x => x.ResourceLinkID,
                        principalTable: "ResourceLinks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResourceMappings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CrawledLinkID = table.Column<int>(type: "INTEGER", nullable: false),
                    FoundLinkID = table.Column<int>(type: "INTEGER", nullable: false),
                    ResourceLinkID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceMappings", x => x.ID);
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
                    table.ForeignKey(
                        name: "FK_ResourceMappings_ResourceLinks_ResourceLinkID",
                        column: x => x.ResourceLinkID,
                        principalTable: "ResourceLinks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ETCLinksMetadata_ResourceLinkID",
                table: "ETCLinksMetadata",
                column: "ResourceLinkID");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceLinks_ApplicationID",
                table: "ResourceLinks",
                column: "ApplicationID");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceMappings_CrawledLinkID",
                table: "ResourceMappings",
                column: "CrawledLinkID");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceMappings_FoundLinkID",
                table: "ResourceMappings",
                column: "FoundLinkID");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceMappings_ResourceLinkID",
                table: "ResourceMappings",
                column: "ResourceLinkID");

            migrationBuilder.CreateIndex(
                name: "IX_RunInfo_ApplicationID",
                table: "RunInfo",
                column: "ApplicationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ETCLinksMetadata");

            migrationBuilder.DropTable(
                name: "ResourceMappings");

            migrationBuilder.DropTable(
                name: "RunInfo");

            migrationBuilder.DropTable(
                name: "ResourceLinks");

            migrationBuilder.DropTable(
                name: "Applications");
        }
    }
}
