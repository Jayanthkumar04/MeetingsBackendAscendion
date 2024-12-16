﻿using System.Text.Json.Serialization;

namespace MeetingsBackendAscendion.Models.Domain
{
    public class Meeting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public List<MeetingAttendee> Attendees { get; set; }
    }
    
}
