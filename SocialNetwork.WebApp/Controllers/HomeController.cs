using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using SocialNetwork.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Globalization;
using System.Collections.Generic;

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
            await RefreshToken();
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await client.GetAsync($"http://localhost:5001/api/shouts");

                if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return View("Unauthorized");
                }

                var shoutsResponse = await response.Content.ReadAsStringAsync();
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

        public async Task RefreshToken()
        {
            var authorizationServerInformation = await DiscoveryClient.GetAsync("http://localhost:5000");
            var client = new TokenClient(authorizationServerInformation.TokenEndpoint, "socialnetwork_hybrid", "client_secret");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            var tokenResponse = await client.RequestRefreshTokenAsync(refreshToken);
            var identityToken = await HttpContext.GetTokenAsync("id_token");

            var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResponse.ExpiresIn);

            var tokens = new[]
            {
                new AuthenticationToken()
                {
                    Name = OpenIdConnectParameterNames.IdToken,
                    Value = identityToken
                },
                new AuthenticationToken()
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = tokenResponse.AccessToken
                },
                new AuthenticationToken()
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = tokenResponse.RefreshToken
                },
                new AuthenticationToken()
                {
                    Name = "expires_at",
                    Value = expiresAt.ToString("o", CultureInfo.InvariantCulture)
                }
            };

            var authenticationInformation = await HttpContext.AuthenticateAsync("Cookies");
            authenticationInformation.Properties.StoreTokens(tokens);
            await HttpContext.SignInAsync("Cookies", authenticationInformation.Principal, authenticationInformation.Properties);
        }
    }
}
