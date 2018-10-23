using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using SocialNetwork.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;

namespace SocialNetwork.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> Shouts()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
                var shoutsResponse = await (await client.GetAsync($"http://localhost:5001/api/shouts")).Content.ReadAsStringAsync();

                var shouts = JsonConvert.DeserializeObject<Shout[]>(shoutsResponse);
                
                return View(shouts);
            }
        }

        //public IActionResult Login()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Login(string username, string password)
        //{
        //    // NEVER DO THIS
        //    // NEVER DO THIS
        //    // NEVER DO THIS
        //    // NEVER DO THIS
        //    HttpContext.Response.Cookies.Append("username", username);

        //    // NEVER DO THIS
        //    // NEVER DO THIS
        //    // NEVER DO THIS
        //    HttpContext.Response.Cookies.Append("password", password);

        //    return RedirectToAction("Shouts");
        //}

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }
    }
}
