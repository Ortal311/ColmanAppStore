using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanAppStore.Models
{
    public class PaymentMethod
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Name on card")]
        [Required(ErrorMessage = "You must enter name")]
        public string NameOnCard { get; set; }
        
        [Display(Name = "Card number")]
        [Required(ErrorMessage = "You must enter credit card number")]
        public long CardNumber { get; set; }

        [Display(Name = "Expired date")]
        [Required(ErrorMessage = "You must enter expired date")]
        public String ExpiredDate { get; set; }

        [Required(ErrorMessage = "You must enter cvv")]
        [Range(100,9999,ErrorMessage = "The password must be at 3 or 4 digits")]
        public int CVV { get; set; }

        [Display(Name = "ID number")]
        [Required(ErrorMessage = "You must enter Id")]
        [Range(100000000, 999999999, ErrorMessage = "The password must be 9 characters")]
        public long IdNumber { get; set; }

        //One to many
        public List<Payment> Payments { get; set; }

        //Many to many
        [Display(Name = "Choose users who can use the card")]
        public List<User> Users { get; set; }
    }
}
