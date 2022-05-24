namespace Northwind.Services.EntityFrameworkCore.Blogging.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "blog_article",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    posted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    employee_id = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_article", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "blog_article_product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    BlogArticleId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_article_product", x => x.id);
                    table.ForeignKey(
                        name: "FK_blog_article_product_blog_article_BlogArticleId",
                        column: x => x.BlogArticleId,
                        principalTable: "blog_article",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_blog_article_product_BlogArticleId",
                table: "blog_article_product",
                column: "BlogArticleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blog_article_product");

            migrationBuilder.DropTable(
                name: "blog_article");
        }
    }
}
