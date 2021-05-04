using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanAppStore.Models
{
    public class Apps
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
        public Logo Logo { get; set; }

        public List<AppsImage> Images { get; set; }

        [Range(0,5)]
        public float Raiting { get; set; }

        public Review Review { get; set; }

        [Required]
        [Display(Name="Developer Name")]
        public string DeveloperName { get; set; }
    }
}
