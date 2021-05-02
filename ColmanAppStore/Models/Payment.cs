using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanAppStore.Models
{
    public class Payment
    {

        public int Id { get; set; }

        public long CardNumber { get; set; }

        public int ExpiredDate { get; set; }

        public int CVV { get; set; }

        public string Name { get; set; }

        public long IdNumber { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }


    }
}
