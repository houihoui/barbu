namespace Barbu.Api.DTOs;

public class GameDetailsDto : GameDto
{
    public List<DealDto> Deals { get; set; } = new();
}
