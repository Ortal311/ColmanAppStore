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

        public string Title { get; set; }

        public string Body { get; set; }

        [Range(0, 5)]
        [Required(ErrorMessage = "You must enter raiting")]
        public float Raiting { get; set; }

        public DateTime PublishDate { get; set; }

        [Display(Name = "App name")]
        public int AppId { get; set; }

        public App App { get; set; }



        [Display(Name = "User name")]
        //[Required(ErrorMessage = "You must choose user")]
        public User UserName { get; set; }

    }
}
