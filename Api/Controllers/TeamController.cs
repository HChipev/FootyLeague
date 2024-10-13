using Api.ViewModels.Team;
using Domain.Service.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootyLeague.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet("team")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get()
    {
        var teams = await _teamService.GetAllTeamsAsync();

        return teams.Any() ? Ok(teams) : NotFound();
    }

    [HttpPost("create")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Create(CreateTeamInputModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _teamService.CreateTeamAsync(model);

        return Ok();
    }

    [HttpPost("update_all_teams_stats")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateAllTeamsStats()
    {
        await _teamService.UpdateAllTeamsStats();

        return Ok();
    }

    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get(int id)
    {
        var team = await _teamService.GetTeamAsync(id);

        return Ok(team);
    }

    [HttpDelete("delete/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        await _teamService.Delete(id);

        return Ok();
    }

    [HttpPost("restore/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Restore(int id)
    {
        await _teamService.Restore(id);

        return Ok();
    }

    [HttpPatch("edit/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Patch(int id, [FromBody] EditTeamInputModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _teamService.UpdateAsync(id, model);

        return Ok();
    }
}
