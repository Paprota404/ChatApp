using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddedSenderUsername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "sender_username",
                table: "friend_requests",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sender_username",
                table: "friend_requests");
        }
    }
}
