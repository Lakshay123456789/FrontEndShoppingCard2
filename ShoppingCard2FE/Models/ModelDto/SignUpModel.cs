using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartModels.ModelDto
{
    public class SignUpModel
    {
        [Required]
        public string ? FirstName { get; set; }

        public string ?LastName { get;set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string ? EmailId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ?ConfirmPassword { get; set; }
    }
}
