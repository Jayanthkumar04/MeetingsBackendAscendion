using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingsBackendAscendion.Models.DTO;

public class RegisterRequestDto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int _Id;

    [Required]
    public string Name { set; get; }
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

}
