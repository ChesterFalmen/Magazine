using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazine.Models
{
    public class Promo
    {
        public int Id { get; set; }

        public int? UsingPromoId { get; set; }
        public virtual UsingPromo UsingPromo { get; set; }

        public int? CustomerId { get; set; }    
        public virtual Customer Customer { get; set; }

        public string Name { get; set; }
        public int Sum { get; set; }
    }
}
