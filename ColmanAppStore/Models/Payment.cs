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

        [Required(ErrorMessage = "You must enter name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must enter address")]
        public String Address { get; set; }

        [Required(ErrorMessage = "You must enter city")]
        public String City { get; set; }

        [Required]
        [Display(Name = "Payment methods")]
        public int PaymentMethodId { get; set; }

        public PaymentMethod PaymentMethod { get; set; } //ONE payment method to MANY payments

        [Display(Name = "App name")]
        public int AppId { get; set; }

        public App App { get; set; }
    }
}
