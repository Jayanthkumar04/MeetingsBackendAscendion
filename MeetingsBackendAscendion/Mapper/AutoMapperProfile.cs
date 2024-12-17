using AutoMapper;
using MeetingsBackendAscendion.Models.Domain;
using MeetingsBackendAscendion.Models.DTO;

namespace MeetingsBackendAscendion.Mapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Meeting, MeetingDto>()
                .ForMember(dest => dest.Attendees, opt => opt.MapFrom(src => src.Attendees));

            CreateMap<MeetingAttendee, CustomMeetingAttendees>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email)); // Add mapping for Email

        }

    }
}
