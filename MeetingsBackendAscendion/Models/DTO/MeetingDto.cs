namespace MeetingsBackendAscendion.Models.DTO
{
    public class MeetingDto
    {
        public int Id { get; set; }  // Primary Key
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }


        public ICollection<CustomMeetingAttendees> Attendees { get; set; }
    }
    public class CustomMeetingAttendees
    {


        public string UserId { get; set; }
        public string Email { get; set; }


    }
}

