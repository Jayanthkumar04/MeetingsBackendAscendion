namespace MeetingsBackendAscendion.Models.DTO;

public class LoginResponseDto
{
    public string AuthToken { get; set; }
    public string? Email { get; set; }

    public String Message { get; set; }

    public String Name { get; set; }
}
