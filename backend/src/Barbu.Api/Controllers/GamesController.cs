using Barbu.Api.DTOs;
using Barbu.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Barbu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly IGamesService _gamesService;
    private readonly ILogger<GamesController> _logger;

    public GamesController(IGamesService gamesService, ILogger<GamesController> logger)
    {
        _gamesService = gamesService;
        _logger = logger;
    }

    /// <summary>
    /// Récupère toutes les parties
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GameDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GameDto>>> GetAllGames()
    {
        var games = await _gamesService.GetAllGamesAsync();
        return Ok(games);
    }

    /// <summary>
    /// Récupère une partie par son ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GameDto>> GetGame(Guid id)
    {
        var game = await _gamesService.GetGameByIdAsync(id);

        if (game == null)
            return NotFound(new { message = $"Partie {id} introuvable" });

        return Ok(game);
    }

    /// <summary>
    /// Crée une nouvelle partie
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GameDto>> CreateGame([FromBody] CreateGameDto createGameDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var game = await _gamesService.CreateGameAsync(createGameDto);

            return CreatedAtAction(
                nameof(GetGame),
                new { id = game.Id },
                game
            );
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Démarre une partie
    /// </summary>
    [HttpPost("{id}/start")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StartGame(Guid id)
    {
        try
        {
            var started = await _gamesService.StartGameAsync(id);

            if (!started)
                return NotFound(new { message = $"Partie {id} introuvable" });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Termine une partie
    /// </summary>
    [HttpPost("{id}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CompleteGame(Guid id)
    {
        try
        {
            var completed = await _gamesService.CompleteGameAsync(id);

            if (!completed)
                return NotFound(new { message = $"Partie {id} introuvable" });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Supprime une partie
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteGame(Guid id)
    {
        try
        {
            var deleted = await _gamesService.DeleteGameAsync(id);

            if (!deleted)
                return NotFound(new { message = $"Partie {id} introuvable" });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
