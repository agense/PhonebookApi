using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhonebookData.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Required]
        [MaxLength(80)]
        [MinLength(2)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(120)]
        [MinLength(2)]
        public string LastName { get; set; }

        public virtual ICollection<UserContactList> UserContactList { get; set; }
    }

}
