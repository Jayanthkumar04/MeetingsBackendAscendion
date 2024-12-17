using AutoMapper;
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

        private IMapper _mapper;

        public MeetingsController(ApplicationDbContext db, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _mapper = mapper;
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

            _db.Meetings.Add(meeting);
            await _db.SaveChangesAsync();

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

            await _db.SaveChangesAsync();

            var meetingDto = _mapper.Map<MeetingDto>(meeting);

            return Ok(meetingDto);
        }




        [HttpGet]
        [Route("meetings")]
        public async Task<ActionResult<IEnumerable<MeetingDto>>> GetMeetingsForTheDay(DateTime date)
        {

            var currentUser = await _userManager.GetUserAsync(User);

            if(currentUser == null)
            {
                return Unauthorized("user is not authenticated");
            }
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1);

            var meetings = await _db.Meetings
      .Include(m => m.Attendees)  // Include the Attendees navigation property
      .ToListAsync();

            Console.WriteLine(meetings);

            var meetingDtos = meetings.Select(m => new MeetingDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Date = m.Date,
                StartTime = new TimeOnly(m.StartTime.Hour, m.StartTime.Minute),
                EndTime = new TimeOnly(m.EndTime.Hour, m.EndTime.Minute),
                Attendees = m.Attendees.Select(a => new CustomMeetingAttendees
                {
                    UserId = a.Id,
                    Email = a.Email
                }).ToList()
            }).ToList();

            Console.WriteLine(meetingDtos);

            return Ok(meetingDtos);
        }

        [HttpGet]
        [Route("meetings/filter")]
        public async Task<ActionResult<IEnumerable<MeetingDto>>> GetMeetings([FromQuery] string period, [FromQuery] string search)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            var currentDate = DateTime.Now;

            IQueryable<Meeting> meetingsQuery = _db.Meetings
        .Where(m => m.Attendees.Any(a => a.Id == currentUser.Id));

            switch (period.ToLower())
            {
                case "past":
                    meetingsQuery = meetingsQuery.Where(m => m.Date < currentDate.Date); // Past meetings
                    break;

                case "future":
                    meetingsQuery = meetingsQuery.Where(m => m.Date > currentDate.Date); // Future meetings
                    break;

                case "present":
                    meetingsQuery = meetingsQuery.Where(m => m.Date == currentDate.Date); // Meetings happening today
                    break;

                case "all":
                default:
                    // No filtering needed for 'all'
                    break;
            }
            if (search!=null)
            {
                meetingsQuery = meetingsQuery.Where(m => m.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            var meetings = await meetingsQuery.ToListAsync();

            var meetingDtos = meetings.Select(m => new MeetingDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Date = m.Date,
                StartTime = new TimeOnly(m.StartTime.Hour, m.StartTime.Minute),
                EndTime = new TimeOnly(m.EndTime.Hour, m.EndTime.Minute),
                Attendees = m.Attendees.Select(a => new CustomMeetingAttendees
                {
                    UserId = a.Id,
                    Email = a.Email
                }).ToList()
            }).ToList();

            return Ok(meetingDtos);


        }


        }

}