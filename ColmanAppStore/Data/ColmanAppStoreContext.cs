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

        public DbSet<ColmanAppStore.Models.App> Apps { get; set; }

        public DbSet<ColmanAppStore.Models.Category> Category { get; set; }

        public DbSet<ColmanAppStore.Models.User> User { get; set; }

        public DbSet<ColmanAppStore.Models.AppImage> AppsImage { get; set; }

        public DbSet<ColmanAppStore.Models.Logo> Logo { get; set; }

        public DbSet<ColmanAppStore.Models.Payment> Payment { get; set; }

        public DbSet<ColmanAppStore.Models.AppVideo> AppVideo { get; set; }

        public DbSet<ColmanAppStore.Models.Review> Review { get; set; }

        public DbSet<ColmanAppStore.Models.PaymentMethod> PaymentMethod { get; set; }
    }
}
