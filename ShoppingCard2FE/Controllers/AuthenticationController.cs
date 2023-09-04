using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Packaging.Signing;
using ShoppingCartModels.ModelDto;
using ShoppingCartModels.ResponseModel;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace ShoppingCard2FE.Controllers
{
    public class AuthenticationController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Index(SignUpModel model)
        {
            if (ModelState.IsValid)
            {

                SignUpModel t = new SignUpModel();
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:7226/api/Authenticate/SignUp", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        t = JsonConvert.DeserializeObject<SignUpModel>(apiResponse);
                    }
                }
                return RedirectToAction("Login");

            }
            ModelState.AddModelError("Model", "There is already one like that");
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var loginResponse = new LoginResponse();
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:7226/api/Authenticate/Login", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            loginResponse = JsonConvert.DeserializeObject<LoginResponse>(apiResponse);

                            HttpContext.Session.SetString("Role", loginResponse.Role);
                            HttpContext.Session.SetString("UserName", loginResponse.UserName);
                            HttpContext.Session.SetString("Token", loginResponse.Token);
                            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(loginResponse.Token);

                            var claimsIdentity = new ClaimsIdentity(jwt.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));

                            if (loginResponse.Role == "Admin")
                            {

                            return RedirectToAction("DisplayProducts","Admin");
                            }
                            else
                            {
                            return RedirectToAction("DisplayProducts","User");

                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt. Please try again.");
                        }
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task< ActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}

