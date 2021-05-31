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

        [DataType(DataType.Currency)]
        public float Price { get; set; }

        public string Description { get; set; }

        [Display(Name = "Publish date")]
        public DateTime publishDate { get; set; }

        public Category Category { get; set; }

        [Required]
        [Display(Name = "Category Id")]
        public int CategoryId { get; set; }

        public float Size { get; set; }

        [Required]
        [Display(Name = "App logo")]
        public Logo Logo { get; set; }

        [Display(Name = "App images")]
        public List<AppImage> Images { get; set; }

        [Display(Name = "App videos")]
        public List<AppVideo> Videos { get; set; }

        public float AverageRaiting { get; set; }

        public List<Review> Review { get; set; }

        public int countReview { get; set; } //raiting

        [Required]
        [Display(Name="Developer Name")]
        public string DeveloperName { get; set; }
    }
}
