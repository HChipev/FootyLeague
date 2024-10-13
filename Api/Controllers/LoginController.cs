using Api.ViewModels.Administration.Users;
using Domain.Service.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootyLeague.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class LoginController : ControllerBase
{
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var token = await _loginService.LoginAsync(model.Username, model.Password);

        return Ok(new { Token = token });
    }
}
