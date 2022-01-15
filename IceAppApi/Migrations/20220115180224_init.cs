using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IceAppApi.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IceShopOwner",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    lat = table.Column<decimal>(type: "numeric(20,16)", nullable: false),
                    lng = table.Column<decimal>(type: "numeric(20,16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IceShopOwner", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "IceTaste",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    IceTaste = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IceTaste", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "IceShopOffer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IceShopOwner = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IceTaste = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KindPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IceShopOffer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IceShopOffer_IceShopOwner",
                        column: x => x.IceShopOwner,
                        principalTable: "IceShopOwner",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IceShopOffer_IceTaste",
                        column: x => x.IceTaste,
                        principalTable: "IceTaste",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IceShopOffer_IceShopOwner",
                table: "IceShopOffer",
                column: "IceShopOwner");

            migrationBuilder.CreateIndex(
                name: "IX_IceShopOffer_IceTaste",
                table: "IceShopOffer",
                column: "IceTaste");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IceShopOffer");

            migrationBuilder.DropTable(
                name: "IceShopOwner");

            migrationBuilder.DropTable(
                name: "IceTaste");
        }
    }
}
