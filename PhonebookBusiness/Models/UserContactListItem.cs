using System.ComponentModel.DataAnnotations;

namespace PhonebookBusiness.Models
{
    public class UserContactListItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ContactId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int OwnerId { get; set; }

        public Contact Contact { get; set; }

        public ApplicationUser User { get; set; }

    }
}
