using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab04.Models
{
    public class CategoryStatsViewModel
    {
        public string CategoryName { get; set; }
        public int ProductCount { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal TotalValue { get; set; }
    }
}
