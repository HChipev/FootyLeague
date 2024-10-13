using Api.ViewModels.Match;
using Domain.Service.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootyLeague.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MatchController : ControllerBase
{
    private readonly IMatchService _matchService;
    private readonly ITeamService _teamService;

    public MatchController(IMatchService matchService, ITeamService teamService)
    {
        _matchService = matchService;
        _teamService = teamService;
    }

    [HttpGet("match")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get()
    {
        var matches = await _matchService.GetAllMatchesAsync();

        return matches.Any() ? Ok(matches) : NotFound();
    }

    [HttpPost("create")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Create(CreateMatchInputModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _matchService.CreateMatchAsync(model);
        await _teamService.UpdateAllTeamsStats();

        return Ok();
    }

    [HttpDelete("delete/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        await _matchService.Delete(id);

        return Ok();
    }

    [HttpPost("restore/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Restore(int id)
    {
        await _matchService.Restore(id);

        return Ok();
    }

    [HttpPatch("edit/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Patch(int id, [FromBody] EditMatchInputModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _matchService.UpdateAsync(id, model);

        return Ok();
    }
}
