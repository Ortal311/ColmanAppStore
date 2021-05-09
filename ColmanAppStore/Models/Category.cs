using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanAppStore.Models
{
    public class Category
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        
        public List<App> Apps  { get; set; }


    }
}
