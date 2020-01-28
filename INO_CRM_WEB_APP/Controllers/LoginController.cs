using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using INO_CRM_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using INO_CRM_WEB_APP.Helpers;

namespace INO_CRM_WEB_APP.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        
        public async Task<IActionResult> Login(UserModel user)
        {
            string token = "";            
            string apiUrl = "http://localhost:50060/";
            HttpClient client = new HttpClient();

            user.Password = EncryptionHelper.GetMd5Hash(user.Password);

            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.PostAsJsonAsync("api/auth/token", user);
            if (response.IsSuccessStatusCode)
            {
                token = response.Content.ReadAsStringAsync().Result;
            }

            if(token != "")
            {
                token = token.Remove(token.Length - 1);
                token = token.Substring(1);
                //string uToken = Regex.Unescape(token);
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

                HttpContext.Session.SetString("login", jwtToken.Audiences.ToArray()[0]);
                HttpContext.Session.SetString("token", token);
                HttpContext.Session.SetString("role", jwtToken.Claims.First(x => x.Type.ToString().Equals(ClaimTypes.Role)).Value);
                

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.LoginError = "Bad username or password";
                return View("Index");
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}