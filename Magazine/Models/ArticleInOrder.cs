﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazine.Models
{
    public class ArticleInOrder
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int? ArticleId { get; set; }
        public Article Article { get; set; }
        public Order Order { get; set; }
        public int Price { get; set; } 
    }
}
