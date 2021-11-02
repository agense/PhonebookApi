using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhonebookData.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        [MinLength(2)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(120)]
        [MinLength(2)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(5)]
        [MinLength(1)]
        public string CountryCode{ get; set; }

        [Required]
        [MaxLength(10)]
        [MinLength(4)]
        public string Phone { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserContactList> UserContactList { get; set; }

        [NotMapped]
        public int? OwnerId { get; set; } = null;
    }
}
