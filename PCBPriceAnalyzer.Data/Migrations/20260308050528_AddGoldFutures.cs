using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCBPriceAnalyzer.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGoldFutures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoldFuturesRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Open = table.Column<decimal>(type: "TEXT", nullable: false),
                    High = table.Column<decimal>(type: "TEXT", nullable: false),
                    Low = table.Column<decimal>(type: "TEXT", nullable: false),
                    Close = table.Column<decimal>(type: "TEXT", nullable: false),
                    AdjClose = table.Column<decimal>(type: "TEXT", nullable: false),
                    Volume = table.Column<long>(type: "INTEGER", nullable: false),
                    Ma7 = table.Column<decimal>(type: "TEXT", nullable: false),
                    Ma30 = table.Column<decimal>(type: "TEXT", nullable: false),
                    Ma90 = table.Column<decimal>(type: "TEXT", nullable: false),
                    DailyReturn = table.Column<decimal>(type: "TEXT", nullable: false),
                    Volatility7 = table.Column<decimal>(type: "TEXT", nullable: false),
                    Volatility30 = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rsi = table.Column<decimal>(type: "TEXT", nullable: false),
                    Macd = table.Column<decimal>(type: "TEXT", nullable: false),
                    MacdSignal = table.Column<decimal>(type: "TEXT", nullable: false),
                    BbUpper = table.Column<decimal>(type: "TEXT", nullable: false),
                    BbLower = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoldFuturesRecords", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoldFuturesRecords");
        }
    }
}
