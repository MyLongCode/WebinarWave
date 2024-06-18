
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebinarWave.Controllers.User;
using WebinarWave.Data;
using WebinarWave.Models;

namespace WebinarWave.Controllers
{
    public class AccountController : Controller
    {
        public ApplicationDbContext db;
        public AccountController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpPost("/token")]
        public IActionResult Token(GetUserTokenRequest dto)
        {
            var identity = GetIdentity(dto.Username, dto.Username);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("/register")]
        public IActionResult RegisterUser(UserRegisterRequest dto)
        {
            var user = db.Users.FirstOrDefault(t => t.Username == dto.Username);
            if (user != null)
            {
                return BadRequest("user with this username is already registration");
            }
            db.Users.Add(new Models.User
            {
                Username = dto.Username,
                Password = dto.Password,
            });
            db.SaveChanges();
            GetIdentity(dto.Username, dto.Password);

        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var user = db.Users.FirstOrDefault(t => t.Username == username && t.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}