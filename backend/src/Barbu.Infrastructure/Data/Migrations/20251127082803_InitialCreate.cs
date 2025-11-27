using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Barbu.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Championships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Championships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Avatar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    GamesPlayed = table.Column<int>(type: "integer", nullable: false),
                    Wins = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PlayerCount = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CurrentDealNumber = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChampionshipId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Championships_ChampionshipId",
                        column: x => x.ChampionshipId,
                        principalTable: "Championships",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChampionshipPlayers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChampionshipId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalPoints = table.Column<decimal>(type: "numeric(10,1)", precision: 10, scale: 1, nullable: false),
                    GamesPlayed = table.Column<int>(type: "integer", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChampionshipPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChampionshipPlayers_Championships_ChampionshipId",
                        column: x => x.ChampionshipId,
                        principalTable: "Championships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChampionshipPlayers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GamePlayers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    TotalScore = table.Column<int>(type: "integer", nullable: false),
                    RemainingChallenges = table.Column<int>(type: "integer", nullable: false),
                    RemainingSurcontres = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GamePlayers_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePlayers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Deals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    DealNumber = table.Column<int>(type: "integer", nullable: false),
                    DeclarerGamePlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContractType = table.Column<int>(type: "integer", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deals_GamePlayers_DeclarerGamePlayerId",
                        column: x => x.DeclarerGamePlayerId,
                        principalTable: "GamePlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deals_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Challenges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DealId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ChallengerGamePlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChallengedGamePlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScoreDifference = table.Column<int>(type: "integer", nullable: false),
                    PointsTransferred = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Challenges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Challenges_Deals_DealId",
                        column: x => x.DealId,
                        principalTable: "Deals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Challenges_GamePlayers_ChallengedGamePlayerId",
                        column: x => x.ChallengedGamePlayerId,
                        principalTable: "GamePlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Challenges_GamePlayers_ChallengerGamePlayerId",
                        column: x => x.ChallengerGamePlayerId,
                        principalTable: "GamePlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DealScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DealId = table.Column<Guid>(type: "uuid", nullable: false),
                    GamePlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    BaseScore = table.Column<int>(type: "integer", nullable: false),
                    FinalScore = table.Column<int>(type: "integer", nullable: false),
                    ScoreDetails = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealScores_Deals_DealId",
                        column: x => x.DealId,
                        principalTable: "Deals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealScores_GamePlayers_GamePlayerId",
                        column: x => x.GamePlayerId,
                        principalTable: "GamePlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_ChallengedGamePlayerId",
                table: "Challenges",
                column: "ChallengedGamePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_ChallengerGamePlayerId",
                table: "Challenges",
                column: "ChallengerGamePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_DealId_ChallengerGamePlayerId_ChallengedGamePlay~",
                table: "Challenges",
                columns: new[] { "DealId", "ChallengerGamePlayerId", "ChallengedGamePlayerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChampionshipPlayers_ChampionshipId_PlayerId",
                table: "ChampionshipPlayers",
                columns: new[] { "ChampionshipId", "PlayerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChampionshipPlayers_PlayerId",
                table: "ChampionshipPlayers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Championships_IsActive",
                table: "Championships",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Championships_Name",
                table: "Championships",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_DeclarerGamePlayerId",
                table: "Deals",
                column: "DeclarerGamePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_GameId_DealNumber",
                table: "Deals",
                columns: new[] { "GameId", "DealNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DealScores_DealId_GamePlayerId",
                table: "DealScores",
                columns: new[] { "DealId", "GamePlayerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DealScores_GamePlayerId",
                table: "DealScores",
                column: "GamePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_GameId_PlayerId",
                table: "GamePlayers",
                columns: new[] { "GameId", "PlayerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_GameId_Position",
                table: "GamePlayers",
                columns: new[] { "GameId", "Position" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_PlayerId",
                table: "GamePlayers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_ChampionshipId",
                table: "Games",
                column: "ChampionshipId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Status",
                table: "Games",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Players_Name",
                table: "Players",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Challenges");

            migrationBuilder.DropTable(
                name: "ChampionshipPlayers");

            migrationBuilder.DropTable(
                name: "DealScores");

            migrationBuilder.DropTable(
                name: "Deals");

            migrationBuilder.DropTable(
                name: "GamePlayers");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Championships");
        }
    }
}
