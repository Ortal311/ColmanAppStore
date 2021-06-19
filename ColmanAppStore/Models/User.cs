using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanAppStore.Models
{
    public enum UserType
    {
        Client,
        Programer,
        Admin
    }
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "User name")]
        [StringLength(15, MinimumLength= 5)]
        [Required(ErrorMessage = "You must enter user name")]
        public string Name { get; set; } //user name


        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "You must enter email")]
        public string Email { get; set; }

        
        [StringLength(30, MinimumLength = 8,ErrorMessage ="The password must be at least 8 charcaters")]
        [Required(ErrorMessage ="You must enter password")]
        [RegularExpression("^[A-Z]+[a-zA-Z0-9 ]*$", ErrorMessage = "The body must start with one or more uppercase letters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public List<PaymentMethod> PaymentMethods { get; set; } //MANY users to MANY payment methods

        public List<App> AppListUser { get; set; } //MANY users to MANY apps

        public UserType UserType { get; set; } = UserType.Client;
    }
}
