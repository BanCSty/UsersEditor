using System.ComponentModel.DataAnnotations;

namespace UsersEditor.Models
{
    public class User
    {
        public Guid Guid { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public int Gender { get; set; }

        public DateTime Birthday { get; set; }

        public bool Admin { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreateBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime RevokedOn { get; set; }

        public string RevokedBy { get; set; }
    }

    public record UserLoginDTO
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public record UserCreateDTO
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Gender { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        public bool Admin { get; set; }
    }

    public record UserLogNameGenderBirthdayDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Gender { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        public bool State { get; set; }
    }

}
