using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazine.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        public int ProducerId { get; set; }
        public Producer Producer { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public virtual List<ArticleInOrder> ArticleInOrders { get; set; } 
    }
}
