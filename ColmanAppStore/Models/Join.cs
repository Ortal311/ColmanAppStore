using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ColmanAppStore.Models
{
    //View Model for join
    public class Join
    {
        [ForeignKey("UserID")]
        public int ID { get; set; }

        [NotMapped]
        public List<Payment> Users { get; set; } //list of users that bought the app- only admin can see it

        [NotMapped]
        public List<Review> UserReviews { get; set; } //every client will see all the reviews he wrote


    }
}
