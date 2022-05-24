namespace Northwind.Services.EntityFrameworkCore.Blogging.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedBlogArticleCommentPlusChangedNamingInBAP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_blog_article_product_blog_article_BlogArticleId",
                table: "blog_article_product");

            migrationBuilder.RenameColumn(
                name: "BlogArticleId",
                table: "blog_article_product",
                newName: "blog_article_id");

            migrationBuilder.RenameIndex(
                name: "IX_blog_article_product_BlogArticleId",
                table: "blog_article_product",
                newName: "IX_blog_article_product_blog_article_id");

            migrationBuilder.CreateTable(
                name: "blog_article_comments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    blog_article_id = table.Column<int>(type: "int", nullable: false),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_article_comments", x => x.id);
                    table.ForeignKey(
                        name: "FK_blog_article_comments_blog_article_blog_article_id",
                        column: x => x.blog_article_id,
                        principalTable: "blog_article",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_blog_article_comments_blog_article_id",
                table: "blog_article_comments",
                column: "blog_article_id");

            migrationBuilder.AddForeignKey(
                name: "FK_blog_article_product_blog_article_blog_article_id",
                table: "blog_article_product",
                column: "blog_article_id",
                principalTable: "blog_article",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_blog_article_product_blog_article_blog_article_id",
                table: "blog_article_product");

            migrationBuilder.DropTable(
                name: "blog_article_comments");

            migrationBuilder.RenameColumn(
                name: "blog_article_id",
                table: "blog_article_product",
                newName: "BlogArticleId");

            migrationBuilder.RenameIndex(
                name: "IX_blog_article_product_blog_article_id",
                table: "blog_article_product",
                newName: "IX_blog_article_product_BlogArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_blog_article_product_blog_article_BlogArticleId",
                table: "blog_article_product",
                column: "BlogArticleId",
                principalTable: "blog_article",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
