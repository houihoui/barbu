using Barbu.Api.DTOs;
using Barbu.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Barbu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly IPlayersService _playersService;
    private readonly ILogger<PlayersController> _logger;

    public PlayersController(IPlayersService playersService, ILogger<PlayersController> logger)
    {
        _playersService = playersService;
        _logger = logger;
    }

    /// <summary>
    /// Récupère tous les joueurs
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PlayerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PlayerDto>>> GetAllPlayers()
    {
        var players = await _playersService.GetAllPlayersAsync();
        return Ok(players);
    }

    /// <summary>
    /// Récupère un joueur par son ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PlayerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlayerDto>> GetPlayer(Guid id)
    {
        var player = await _playersService.GetPlayerByIdAsync(id);

        if (player == null)
            return NotFound(new { message = $"Joueur {id} introuvable" });

        return Ok(player);
    }

    /// <summary>
    /// Crée un nouveau joueur
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PlayerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PlayerDto>> CreatePlayer([FromBody] CreatePlayerDto createPlayerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var player = await _playersService.CreatePlayerAsync(createPlayerDto);

        return CreatedAtAction(
            nameof(GetPlayer),
            new { id = player.Id },
            player
        );
    }

    /// <summary>
    /// Met à jour un joueur existant
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PlayerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlayerDto>> UpdatePlayer(Guid id, [FromBody] UpdatePlayerDto updatePlayerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var player = await _playersService.UpdatePlayerAsync(id, updatePlayerDto);

        if (player == null)
            return NotFound(new { message = $"Joueur {id} introuvable" });

        return Ok(player);
    }

    /// <summary>
    /// Supprime un joueur
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeletePlayer(Guid id)
    {
        try
        {
            var deleted = await _playersService.DeletePlayerAsync(id);

            if (!deleted)
                return NotFound(new { message = $"Joueur {id} introuvable" });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
