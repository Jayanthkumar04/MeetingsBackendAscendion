using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MeetingsBackendAscendion.Models.DTO;
//using MeetingsBackendAscendion.CustomActionFilters;
using MeetingsBackendAscendion.Repositories;
using MeetingsBackendAscendion.Models.DTO;
using MeetingsBackendAscendion.Repositories;

namespace AscendionAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly ITokenRepository tokenRepository;

    public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
    {
        this.userManager = userManager;
        this.tokenRepository = tokenRepository;
    }

    // POST: /api/Auth/Register
    [HttpPost]
    [Route("Register")]
    //[ValidateModel]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        var identityUser = new IdentityUser
        {
            UserName = registerRequestDto.Name,
            Email = registerRequestDto.Username,
          
        };

        var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

        if (identityResult.Succeeded)
        {
            

                    return Ok("User was registered! Please login.");
            
        }

        return BadRequest("Something went wrong");
    }

    // POST: /api/Auth/Login
    [HttpPost]
    [Route("Login")]
    //[ValidateModel]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

        if (user != null)
        {
            var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (checkPasswordResult)
            {
                // Get Roles for this user
               
                    var authToken = tokenRepository.CreateJWTToken(user);

                var response = new LoginResponseDto
                {
                    AuthToken = authToken,
                    Email = user.Email,
                    Message = "login is successfull",

                };

                    return Ok(response);
                
            }
        }

        return BadRequest("Username or password incorrect");
    }
}