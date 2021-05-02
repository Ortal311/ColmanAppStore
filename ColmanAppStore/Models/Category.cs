using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanAppStore.Models
{
    public class Category
    {

        //key
        public int Id { get; set; }


        public string Name { get; set; }

        public List<Apps> Apps  { get; set; }


    }
}
