using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazine.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UsingPromoId { get; set; }
        public UsingPromo UsingPromo { get; set; }

        public List<ArticleInOrder> ArticleInOrders { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int Count { get; set; }
        public int Sum { get; set; }
    }
}
