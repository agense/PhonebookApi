using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhonebookBusiness.Models
{
    public class Contact
    {
        [Key]
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
        [MaxLength(5)]
        [MinLength(1)]
        [RegularExpression(@"^[0-9 ()+]+$", ErrorMessage = "Country codes can only contain numbers and a plus sign")]
        public string CountryCode { get; set; }

        [Required]
        [MaxLength(10)]
        [MinLength(4)]
        [RegularExpression(@"\d+", ErrorMessage = "Phone numbers can only contain numbers")]
        public string Phone { get; set; }

        public int? OwnerId { get; set; } = null;

        [JsonIgnore]
        public ICollection<UserContactListItem> UserContactListItems { get; set; }
    }
}
