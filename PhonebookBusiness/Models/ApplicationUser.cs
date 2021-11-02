using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhonebookBusiness.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Last name can only contain alphabetical letters and spaces.")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(120)]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Last name can only contain alphabetical letters and spaces.")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$", ErrorMessage = "Password must be at least 6 characters long and contain one uppercase letter, one lowercase letter and one digit")]
        public string Password { get; set; }

        public ICollection<UserContactListItem> UserContactListItems { get; set; }
    }
}
