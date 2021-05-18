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

        public int Id { get; set; }

        [StringLength(15, MinimumLength= 5)]
        [Required(ErrorMessage = "You must enter user name")]
        public string Name { get; set; }


        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "You must enter email")]
        public string Email { get; set; }

        
        [StringLength(30, MinimumLength = 8,ErrorMessage ="The password must be at least 8 charcaters")]
        [Required(ErrorMessage ="You must enter password")]
        [RegularExpression("^[A-Z]+[a-zA-Z0-9 ]*$", ErrorMessage = "The body must start with one or more uppercase letters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //Many to many
        public List<Payment> PaymentMethod;

        public List<App> AppListUser { get; set; }

        public UserType UserType { get; set; } = UserType.Client;
    }
}
