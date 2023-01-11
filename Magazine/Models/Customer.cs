using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazine.Models
{
    public class Customer: Person
    {
        public virtual List<Order> Orders { get; set; }
        public virtual List<Promo> Promos { get; set; }
        public string Surname { get; set; }

        public List<Order> Orders { get; set; }
        public List<Promo> Promos { get; set; }
    }
}
