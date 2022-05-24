namespace Northwind.Services.EntityFrameworkCore.Blogging.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ChangedCustomerIdToStringInBlogArticleComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "customer_id",
                table: "blog_article_comments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "customer_id",
                table: "blog_article_comments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
