using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ShoppingCard2FE.Models;
using ShoppingCartModels.EntityModels;
using ShoppingCartModels.ModelDto;
using System.Net.Http;
using System.Text;

namespace ShoppingCard2FE.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
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
        [HttpGet]
        public ActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddProduct(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string token = HttpContext.Session.GetString("Token");
                    using (var httpClient = new HttpClient())
                    {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                        httpClient.BaseAddress = new Uri("https://localhost:7226/api/Product/");

                        var jsonContent = JsonConvert.SerializeObject(model);
                        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync("AddNewProduct", content);

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("DisplayProducts", "Admin");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Error while adding the product.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            string token = HttpContext.Session.GetString("Token");
            try
            {
                string apiUrl = $"https://localhost:7226/api/Product/GetProductById?id={id}";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var product = JsonConvert.DeserializeObject<Product>(jsonResponse);

                        return View(product);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "API request failed. Product not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            }

            return RedirectToAction("Error");
        }
        [HttpPost]
        public async Task<ActionResult> Edit(Product product)
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                string apiUrl = "https://localhost:7226/api/Product/UpdateProduct";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    var jsonContent = JsonConvert.SerializeObject(product);
                    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync($"{apiUrl}?Id={product.Id}", httpContent);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("DisplayProducts", "Admin"); // Redirect to a list of products or another suitable action
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "API request failed. Please try again later.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            }

            return View(product);
        }

        [HttpGet]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                string apiUrl = $"https://localhost:7226/api/Product/GetProductById?id={id}";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var product = JsonConvert.DeserializeObject<Product>(jsonResponse);

                        return View(product);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "API request failed. Product not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            }

            return RedirectToAction("Error");
        }
        [HttpPost]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                string apiUrl = $"https://localhost:7226/api/Product/DeleteProduct?id={id}";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    HttpResponseMessage response = await client.DeleteAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("DisplayProducts"); // Redirect to a list of products or another suitable action
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "API request failed. Product not found or could not be deleted.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            }

            return RedirectToAction("Error");
        }



    }
}
