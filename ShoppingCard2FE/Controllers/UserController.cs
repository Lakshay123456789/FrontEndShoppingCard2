using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppingCartModels.EntityModels;
using System.Data;

namespace ShoppingCard2FE.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        public async Task<IActionResult> DisplayProducts()

        {
               string token = HttpContext.Session.GetString("Token");

            List<Product> products = new List<Product>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                using (var response = await httpClient.GetAsync("https://localhost:7226/api/Product/Getallproducts"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        products = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
                    }
                    else
                    {
                    }
                }
            }
            return View(products);
        }
    }
}
