using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanAppStore.Models
{
    public class Review
    {

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        [Range(0, 5)]
        [Required(ErrorMessage = "You must enter raiting")]
        public float Raiting { get; set; }
        
        [Display(Name = "Publish date")]
        public DateTime PublishDate { get; set; }

        [Display(Name = "App name")]
        public int AppId { get; set; }

        public App App { get; set; } //ONE app to MANY review

        [Display(Name = "User name")]
        public int UserNameId { get; set; }

        [Display(Name = "User name")]
        public User UserName { get; set; } 

    }
}
