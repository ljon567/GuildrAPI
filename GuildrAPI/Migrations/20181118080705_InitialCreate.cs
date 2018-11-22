using Microsoft.EntityFrameworkCore.Migrations;

namespace GuildrAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProfileItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Class = table.Column<string>(nullable: true),
                    Uploaded = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileItem", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "PartyItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PartyName = table.Column<string>(nullable: true),
                    Uploaded = table.Column<string>(nullable: true),
                    Organizer = table.Column<string>(nullable: true),
                    MemberOne = table.Column<string>(nullable: true),
                    MemberTwo = table.Column<string>(nullable: true),
                    MemberThree = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyItem", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileItem");
            migrationBuilder.DropTable(
                name: "PartyItem");
        }
    }
}
