using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberHealthRecordRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ValidPhoneNumber1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "ValidPhoneNumber",
                table: "Members");

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "HealthRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddCheckConstraint(
                name: "ValidPhoneNumber1",
                table: "Trainers",
                sql: " [Phone] LIKE '01[0125][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'");

            migrationBuilder.AddCheckConstraint(
                name: "ValidPhoneNumber",
                table: "Members",
                sql: " [Phone] LIKE '01[0125][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'");

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecords_MemberId",
                table: "HealthRecords",
                column: "MemberId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthRecords_Members_MemberId",
                table: "HealthRecords",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthRecords_Members_MemberId",
                table: "HealthRecords");

            migrationBuilder.DropCheckConstraint(
                name: "ValidPhoneNumber1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "ValidPhoneNumber",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_HealthRecords_MemberId",
                table: "HealthRecords");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "HealthRecords");

            migrationBuilder.AddCheckConstraint(
                name: "ValidPhoneNumber1",
                table: "Trainers",
                sql: " [Phone] like '01%' and Phone not like '%[^0-9]%'");

            migrationBuilder.AddCheckConstraint(
                name: "ValidPhoneNumber",
                table: "Members",
                sql: " [Phone] like '01%' and Phone not like '%[^0-9]%'");
        }
    }
}
