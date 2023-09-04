using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartModels.EntityModels
{
    public class Product
    {
        public Guid Id { get; set; }
        [Required]
        public string? ProductName { get; set; }
        [Required]
        public string? ProductDescription { get; set; }
        [Required]
        public int? ProductQuantity { get; set; }
        [Required]
        public Decimal? ProductPrice { get; set; }
        [Required]
        public string? ProductCatagory { get; set; }

    }
}
