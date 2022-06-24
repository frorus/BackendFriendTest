using System.ComponentModel.DataAnnotations;

namespace BackendFriend.Contracts
{
    public class LoginUserRequest
    {
        [Required]
        public string? Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
