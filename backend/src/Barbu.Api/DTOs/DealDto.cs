using Barbu.Domain.Enums;

namespace Barbu.Api.DTOs;

public class DealDto
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public int DealNumber { get; set; }
    public Guid DeclarerGamePlayerId { get; set; }
    public string DeclarerPlayerName { get; set; } = string.Empty;
    public ContractType ContractType { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public List<DealScoreDto> Scores { get; set; } = new();
    public List<ChallengeDto> Challenges { get; set; } = new();
}
