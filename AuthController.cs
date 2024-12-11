using BlogApp;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(string username, string password)
    {
        var user = await _authService.RegisterAsync(username, password);
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string username, string password)
    {
        var token = await _authService.LoginAsync(username, password);
        return Ok(new { Token = token });
    }
}

