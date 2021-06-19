using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanAppStore.Models
{
    public class App
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public float Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Publish date")]
        public DateTime publishDate { get; set; }

        public Category Category { get; set; } //ONE category to MANY apps

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        
        [Required]
        public float Size { get; set; }

        [Required]
        [Display(Name = "App logo")]
        public Logo Logo { get; set; } //ONE app to ONE logo

        [Display(Name = "Choose 3 app images")]
        public List<AppImage> Images { get; set; } //ONE app to MANY images

        [Display(Name = "Choose 1 app video")]
        public List<AppVideo> Videos { get; set; } //ONE app to ONE video

        public float AverageRaiting { get; set; }

        public List<Review> Review { get; set; } //ONE app to MANY reviews

        public int countReview { get; set; }

        [Required]
        [Display(Name="Developer Name")]
        public string DeveloperName { get; set; }

        public List<User> Users { get; set; } //MANY users to MANY apps

    }
}
