using Barbu.Api.DTOs;
using Barbu.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Barbu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChampionshipsController : ControllerBase
{
    private readonly IChampionshipsService _championshipsService;
    private readonly ILogger<ChampionshipsController> _logger;

    public ChampionshipsController(IChampionshipsService championshipsService, ILogger<ChampionshipsController> logger)
    {
        _championshipsService = championshipsService;
        _logger = logger;
    }

    /// <summary>
    /// Récupère tous les championnats
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ChampionshipDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ChampionshipDto>>> GetAllChampionships()
    {
        var championships = await _championshipsService.GetAllChampionshipsAsync();
        return Ok(championships);
    }

    /// <summary>
    /// Récupère un championnat par son ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ChampionshipDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ChampionshipDto>> GetChampionship(Guid id)
    {
        var championship = await _championshipsService.GetChampionshipByIdAsync(id);

        if (championship == null)
            return NotFound(new { message = $"Championnat {id} introuvable" });

        return Ok(championship);
    }

    /// <summary>
    /// Crée un nouveau championnat
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ChampionshipDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ChampionshipDto>> CreateChampionship([FromBody] CreateChampionshipDto createChampionshipDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var championship = await _championshipsService.CreateChampionshipAsync(createChampionshipDto);

            return CreatedAtAction(
                nameof(GetChampionship),
                new { id = championship.Id },
                championship
            );
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Met à jour un championnat existant
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ChampionshipDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ChampionshipDto>> UpdateChampionship(Guid id, [FromBody] UpdateChampionshipDto updateChampionshipDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var championship = await _championshipsService.UpdateChampionshipAsync(id, updateChampionshipDto);

        if (championship == null)
            return NotFound(new { message = $"Championnat {id} introuvable" });

        return Ok(championship);
    }

    /// <summary>
    /// Démarre un championnat
    /// </summary>
    [HttpPost("{id}/start")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StartChampionship(Guid id)
    {
        try
        {
            var started = await _championshipsService.StartChampionshipAsync(id);

            if (!started)
                return NotFound(new { message = $"Championnat {id} introuvable" });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Termine un championnat
    /// </summary>
    [HttpPost("{id}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CompleteChampionship(Guid id)
    {
        try
        {
            var completed = await _championshipsService.CompleteChampionshipAsync(id);

            if (!completed)
                return NotFound(new { message = $"Championnat {id} introuvable" });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Ajoute un joueur à un championnat
    /// </summary>
    [HttpPost("{id}/players/{playerId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddPlayer(Guid id, Guid playerId)
    {
        try
        {
            var added = await _championshipsService.AddPlayerAsync(id, playerId);

            if (!added)
                return NotFound(new { message = $"Championnat {id} introuvable" });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Retire un joueur d'un championnat
    /// </summary>
    [HttpDelete("{id}/players/{playerId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemovePlayer(Guid id, Guid playerId)
    {
        try
        {
            var removed = await _championshipsService.RemovePlayerAsync(id, playerId);

            if (!removed)
                return NotFound(new { message = $"Joueur {playerId} non trouvé dans le championnat {id}" });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Supprime un championnat
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteChampionship(Guid id)
    {
        try
        {
            var deleted = await _championshipsService.DeleteChampionshipAsync(id);

            if (!deleted)
                return NotFound(new { message = $"Championnat {id} introuvable" });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
