using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanAppStore.Models
{
    public class AppImage
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Image name")]
        public string Name { get; set; }

        [Required]
        public string Image { get; set; }

        [Display(Name = "App name")]
        public int AppId { get; set; }

        [Display(Name = "App name")]
        public App App { get; set; } //ONE app to MANY images

    }
}
