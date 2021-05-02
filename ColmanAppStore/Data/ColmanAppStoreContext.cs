using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ColmanAppStore.Models;

namespace ColmanAppStore.Data
{
    public class ColmanAppStoreContext : DbContext
    {
        public ColmanAppStoreContext (DbContextOptions<ColmanAppStoreContext> options)
            : base(options)
        {
        }

        public DbSet<ColmanAppStore.Models.Apps> Apps { get; set; }
    }
}
