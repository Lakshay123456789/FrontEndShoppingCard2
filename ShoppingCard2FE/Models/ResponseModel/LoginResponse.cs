using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartModels.ResponseModel
{
    public class LoginResponse
    {
        public bool ? Status { get; set; }
        public string ? Token { get; set; }

        public string ? Role { get; set; }

        public string ? UserName { get; set; }
    }
}
