using Microsoft.EntityFrameworkCore.Migrations;

namespace Magazine.Migrations
{
    public partial class ChangeArticleInOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllInvoices_Articles_ArticleId",
                table: "AllInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_AllInvoices_Orders_ArticleId",
                table: "AllInvoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AllInvoices",
                table: "AllInvoices");

            migrationBuilder.RenameTable(
                name: "AllInvoices",
                newName: "AllArticleInOrder");

            migrationBuilder.RenameIndex(
                name: "IX_AllInvoices_ArticleId",
                table: "AllArticleInOrder",
                newName: "IX_AllArticleInOrder_ArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllArticleInOrder",
                table: "AllArticleInOrder",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AllArticleInOrder_Articles_ArticleId",
                table: "AllArticleInOrder",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AllArticleInOrder_Orders_ArticleId",
                table: "AllArticleInOrder",
                column: "ArticleId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllArticleInOrder_Articles_ArticleId",
                table: "AllArticleInOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_AllArticleInOrder_Orders_ArticleId",
                table: "AllArticleInOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AllArticleInOrder",
                table: "AllArticleInOrder");

            migrationBuilder.RenameTable(
                name: "AllArticleInOrder",
                newName: "AllInvoices");

            migrationBuilder.RenameIndex(
                name: "IX_AllArticleInOrder_ArticleId",
                table: "AllInvoices",
                newName: "IX_AllInvoices_ArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllInvoices",
                table: "AllInvoices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AllInvoices_Articles_ArticleId",
                table: "AllInvoices",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AllInvoices_Orders_ArticleId",
                table: "AllInvoices",
                column: "ArticleId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
