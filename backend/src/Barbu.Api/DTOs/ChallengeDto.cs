using Barbu.Domain.Enums;

namespace Barbu.Api.DTOs;

public class ChallengeDto
{
    public Guid Id { get; set; }
    public Guid DealId { get; set; }
    public ChallengeType Type { get; set; }
    public Guid ChallengerGamePlayerId { get; set; }
    public string ChallengerPlayerName { get; set; } = string.Empty;
    public Guid ChallengedGamePlayerId { get; set; }
    public string ChallengedPlayerName { get; set; } = string.Empty;
    public int ScoreDifference { get; set; }
    public int PointsTransferred { get; set; }
}
