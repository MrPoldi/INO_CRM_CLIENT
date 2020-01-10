using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using INO_CRM_API.Models;
using Microsoft.AspNetCore.Mvc;

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
            string token ="Bad token";            
            string apiUrl = "http://localhost:50060/";
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.PostAsJsonAsync("api/auth/token", user);
            if (response.IsSuccessStatusCode)
            {
                token = response.Content.ReadAsStringAsync().Result;
            }
            token = token.Remove(token.Length - 1);
            token = token.Substring(1);
            //string uToken = Regex.Unescape(token);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);


            return Content($"Hello {user.Login} {user.Password} {token} {jwtToken.Issuer} {jwtToken.Audiences.ToArray()[0]}");
        }
    }
}