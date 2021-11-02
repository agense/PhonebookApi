using System.ComponentModel.DataAnnotations;

namespace PhonebookApi.Dto.Requests
{
    public class ContactRequest
    {
        [Required]
        [MaxLength(80)]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "First name can only contain alphabetical letters and spaces.")]
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

    }
}
