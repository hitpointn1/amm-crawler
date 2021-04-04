using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AMMCrawler.DAL.Migrations
{
    public partial class ApplicationInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationID",
                table: "ResourceLinks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_ResourceLinks_ApplicationID",
                table: "ResourceLinks",
                column: "ApplicationID");

            migrationBuilder.CreateIndex(
                name: "IX_ETCLinksMetadata_ResourceLinkID",
                table: "ETCLinksMetadata",
                column: "ResourceLinkID");

            migrationBuilder.CreateIndex(
                name: "IX_RunInfo_ApplicationID",
                table: "RunInfo",
                column: "ApplicationID");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceLinks_Applications_ApplicationID",
                table: "ResourceLinks",
                column: "ApplicationID",
                principalTable: "Applications",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceLinks_Applications_ApplicationID",
                table: "ResourceLinks");

            migrationBuilder.DropTable(
                name: "ETCLinksMetadata");

            migrationBuilder.DropTable(
                name: "RunInfo");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_ResourceLinks_ApplicationID",
                table: "ResourceLinks");

            migrationBuilder.DropColumn(
                name: "ApplicationID",
                table: "ResourceLinks");
        }
    }
}
