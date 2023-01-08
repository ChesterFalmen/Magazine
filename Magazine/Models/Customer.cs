using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazine.Models
{
    public class Customer: Person
    {
        public List<Order> Orders { get; set; }
        public List<Promo> Promos { get; set; }
    }
}
