using Microsoft.EntityFrameworkCore.Migrations;

namespace GlobalGames.Migrations
{
    public partial class InscricoesBd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Inscricoes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inscricoes_UserId",
                table: "Inscricoes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inscricoes_AspNetUsers_UserId",
                table: "Inscricoes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inscricoes_AspNetUsers_UserId",
                table: "Inscricoes");

            migrationBuilder.DropIndex(
                name: "IX_Inscricoes_UserId",
                table: "Inscricoes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Inscricoes");
        }
    }
}
