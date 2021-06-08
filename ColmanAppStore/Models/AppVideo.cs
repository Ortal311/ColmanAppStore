using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanAppStore.Models
{
    public class AppVideo
    { 
        public int Id { get; set; }

        [Required]
        [Display(Name = "Video name")]
        public string Name { get; set; }

        [Required]
        public string Video { get; set; }

        [Display(Name = "App name")]
        public int AppId { get; set; }

        public App App { get; set; }
    }
}
