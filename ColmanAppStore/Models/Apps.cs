using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanAppStore.Models
{
    public class Apps
    {
        //key
        public int Id { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public string Description { get; set; }

        public DateTime publishDate { get; set; }

        public Category Category { get; set; }

        public int CategoryId { get; set; }

        public float Size { get; set; }

        public string Logo { get; set; }

        public string Images { get; set; }

        public float Raiting { get; set; }

        public string Review { get; set; }



        public string DeveloperName { get; set; }
    }
}
