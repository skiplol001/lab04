using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace lab04.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên không được để trống")]
        public string Name { get; set; }
        [Range(1, 1000000, ErrorMessage = "Giá phải > 0")]
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
