using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanAppStore.Models
{
    public class Review
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public string Body  { get; set; }

        public float Raiting { get; set; }

        public DateTime PublishDate { get; set; }

        public User UserName { get; set; }






    }
}
