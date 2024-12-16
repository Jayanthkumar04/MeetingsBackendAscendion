using MeetingsBackendAscendion.Data;
using MeetingsBackendAscendion.Models.Domain;
using MeetingsBackendAscendion.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<Meeting>> AddMeeting([FromBody] CreateMeetingRequest request)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            Console.WriteLine(currentUser);
            if (currentUser == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            var meeting = new Meeting
            {
                Name = request.Name,
                Description = request.Description,
                Date = request.Date,
                StartTime = new TimeOnly(request.StartTime.Hours, request.StartTime.Minutes),
                EndTime = new TimeOnly(request.EndTime.Hours, request.EndTime.Minutes),
                Attendees = new List<MeetingAttendee>() 
            };

            meeting.Attendees.Add(new MeetingAttendee
            {
                Id = currentUser.Id,
                Email = currentUser.Email,
                MeetingId = meeting.Id
            });

            // Add other attendees based on email addresses
            foreach (var email in request.Attendees)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    meeting.Attendees.Add(new MeetingAttendee
                    {
                        Id = user.Id,
                        Email = user.Email,
                        MeetingId = meeting.Id
                    });
                }
            }

            _db.Meetings.Add(meeting);
            await _db.SaveChangesAsync();

            return Ok(meeting);  
        }

    }
}