using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanAppStore.Models
{
    public class Apps
    {
        //key
        public int Id { get; set; }

        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public float Price { get; set; }

        public string Description { get; set; }

        [Display(Name = "Publish date")]
        public DateTime publishDate { get; set; }

        public Category Category { get; set; }

        public int CategoryId { get; set; }

        public float Size { get; set; }

        public string Logo { get; set; }

        public List<AppsImage> Images { get; set; }

        [Range(0,5)]
        public float Raiting { get; set; }

        public Review Review { get; set; }

        [Display(Name="Developer Name")]
        public string DeveloperName { get; set; }
    }
}
