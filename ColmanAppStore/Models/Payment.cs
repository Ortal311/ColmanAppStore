using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanAppStore.Models
{
    public class Payment
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "You must enter card number")]
        public long CardNumber { get; set; }

        [Required(ErrorMessage = "You must enter expired date")]
        public DateTime ExpiredDate { get; set; }

        [Required(ErrorMessage = "You must enter cvv")]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "The password must be at 3 or 4 digits")]
        public int CVV { get; set; }

        [Required(ErrorMessage = "You must enter name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must enter Id")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "The password must be 9 characters")]
        public long IdNumber { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }


    }
}
