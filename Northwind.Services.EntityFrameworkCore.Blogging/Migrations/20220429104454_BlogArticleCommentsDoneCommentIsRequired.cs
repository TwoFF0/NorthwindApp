namespace Northwind.Services.EntityFrameworkCore.Blogging.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class BlogArticleCommentsDoneCommentIsRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "comment",
                table: "blog_article_comments",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "comment",
                table: "blog_article_comments",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400);
        }
    }
}
