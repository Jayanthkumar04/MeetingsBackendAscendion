namespace MeetingsBackendAscendion.Models.Domain;

public class MeetingAttendee
{
    public string Id { get; set; } 
    public string Email { get; set; } 
    public int MeetingId { get; set; }

    public Meeting Meeting { get; set; }
}
