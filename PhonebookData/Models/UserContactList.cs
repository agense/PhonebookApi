using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PhonebookData.Models
{
    public class UserContactList
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ContactId { get; set; }

        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int OwnerId { get; set; }

        public virtual Contact Contact { get; set; }
       
        public virtual ApplicationUser User { get; set; }

    }
}
