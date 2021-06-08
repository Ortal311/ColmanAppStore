using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanAppStore.Models
{
    public class Logo
    {
        public int Id { get; set; }

        [Required]
        public string Image { get; set; }
        
        [Required]
        [Display(Name = "App name")]
        public int AppsId { get; set; }

        [Display(Name = "App name")]
        public App Apps { get; set; }

    }
}
