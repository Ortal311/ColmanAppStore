using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanAppStore.Models
{
    public class AppVideo
    { 
        public int Id { get; set; }

        public string Video { get; set; }

        public int AppId { get; set; }

        public App App { get; set; }
    }
}
