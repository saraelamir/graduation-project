using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduProjj.Migrations
{
    /// <inheritdoc />
    public partial class initDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ProfilePicturePath = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    EmailVerificationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    OtpCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtpExpiration = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    FeedbackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    NavigationEase = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    BudgetHelpfulnessRating = table.Column<int>(type: "int", nullable: false),
                    ConfusingFeatures = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    DesiredFeatures = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    RecommendationReason = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    ImprovementSuggestion = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    OverallSatisfaction = table.Column<int>(type: "int", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.FeedbackId);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Goals",
                columns: table => new
                {
                    GoalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    GoalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Has_Savings = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => x.GoalID);
                    table.ForeignKey(
                        name: "FK_Goals_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inputs",
                columns: table => new
                {
                    InputID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    age = table.Column<int>(type: "int", nullable: false),
                    dependents = table.Column<int>(type: "int", nullable: false),
                    occupation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city_tier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    income = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    rent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    loanPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    insurance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    groceries = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    transport = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    eatingOut = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    entertainment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    utilities = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    healthcare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    education = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    otherMoney = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inputs", x => x.InputID);
                    table.ForeignKey(
                        name: "FK_Inputs_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    PlanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoalID = table.Column<int>(type: "int", nullable: false),
                    MonthlySavings = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Groceries = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Transport = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EatingOut = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Entertainment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Utilities = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Healthcare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Education = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherMoney = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.PlanID);
                    table.ForeignKey(
                        name: "FK_Plans_Goals_GoalID",
                        column: x => x.GoalID,
                        principalTable: "Goals",
                        principalColumn: "GoalID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_SubmissionDate",
                table: "Feedbacks",
                column: "SubmissionDate");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_UserId",
                table: "Feedbacks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_Status",
                table: "Goals",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_UserID",
                table: "Goals",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Inputs_UserID",
                table: "Inputs",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_GoalID",
                table: "Plans",
                column: "GoalID");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_Status",
                table: "Plans",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Inputs");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "Goals");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
