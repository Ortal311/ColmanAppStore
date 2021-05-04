using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanAppStore.Models
{
    public class AppImage
    {

        public int Id { get; set; }

        public string Image { get; set; }

        public int AppId { get; set; }

        public App App { get; set; }



    }
}
