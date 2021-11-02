using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhonebookApi.Dto.Requests
{
    public class ContactSharingRequest
    {
        [Required]
        public int UserId { get; set; }

        //[Required]
        //public bool EnableSharing { get; set; }
    }
}