using MeetingsBackendAscendion.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MeetingsBackendAscendion.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class MeetingsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public MeetingsController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        [HttpGet]
        [Route("users")]
        public IActionResult GetAllUsers()
        {

            var allUsers = _userManager.Users.Select(user => new
            {
                user.Id,
                user.Email,
                user.UserName
            }).ToList();
            return Ok(allUsers);
        }

    }

}