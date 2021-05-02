using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanAppStore.Models
{
    public class AppsImage
    {

        public int Id { get; set; }

        public string Image { get; set; }

        public int AppId { get; set; }

        public Apps App { get; set; }



    }
}
