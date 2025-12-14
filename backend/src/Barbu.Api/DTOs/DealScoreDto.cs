namespace Barbu.Api.DTOs;

public class DealScoreDto
{
    public Guid Id { get; set; }
    public Guid DealId { get; set; }
    public Guid GamePlayerId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public int BaseScore { get; set; }
    public int FinalScore { get; set; }
    public string? ScoreDetails { get; set; }
}
