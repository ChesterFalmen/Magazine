﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazine.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public List<Promo> Promos { get; set; }
    }
}