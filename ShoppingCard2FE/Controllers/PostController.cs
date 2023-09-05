using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using ShoppingCard2FE.Models.EntityModels;
using ShoppingCard2FE.Models.ModelDto;
using ShoppingCartModels.EntityModels;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace ShoppingCard2FE.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class PostController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
    
        public PostController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]

        public IActionResult PostOne()
        {
            ImageDTo imageDTo = new ImageDTo();
            return View(imageDTo);
        }

        [HttpPost]
        public async Task<IActionResult> PostOne(ImageDTo imageDto)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(imageDto.ImageFile.FileName);
            string extension = Path.GetExtension(imageDto.ImageFile.FileName);
            imageDto.ImageName=  fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(wwwRootPath + "/Images/", imageDto.ImageName);
            PostDto postDto = new PostDto
            {
                PostTitle= imageDto.Title,
                PostImage=imageDto.ImageName
            };
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await imageDto.ImageFile.CopyToAsync(fileStream);
            }
            string token = HttpContext.Session.GetString("Token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            StringContent content = new StringContent(JsonConvert.SerializeObject(postDto), Encoding.UTF8, "application/json");

            using (var response = await client.PostAsync("https://localhost:7226/api/Product/addNewPost", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return Redirect("GetImages");
                }
            }
            return View(imageDto);
        
        }

        [HttpGet]

        public async Task<IActionResult> GetImages()
        {
            List<PostDto> postDtos = new List<PostDto>();
            string token = HttpContext.Session.GetString("Token");
            
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            using( var response = await client.GetAsync("https://localhost:7226/api/Product/GetAllImage"))
            {
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    postDtos = JsonConvert.DeserializeObject<List<PostDto>>(jsonResponse);

                    return View(postDtos);
                }
            }
            return View();
        }

    
    }
}
