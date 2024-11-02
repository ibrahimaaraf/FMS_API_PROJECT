using FMS_API_PROJECT.Models;
using FMS_API_PROJECT.Repositories;
using FMS_API_PROJECT.Utilities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FMS_API_PROJECT.Controllers
{
    public class UserController : ApiController
    {
        private readonly IRepository<Login> _loginRepository;
        //BaseController _baseController = new BaseController();
        public UserController()
        {
            _loginRepository = new Repository<Login>(new FMS_DB_Context());
        }

        public string tkn_key = "#@Financial@#@Management@#@System@#";

        [HttpPost]
        [Route("api/login")]
        public async Task<IHttpActionResult> Login(string email, string password)
        {
            var authorizationHeader = HttpContext.Current.Request.Headers["Authorization"];
            string token = null;

            var user = (await _loginRepository.GetAllAsync())
                .FirstOrDefault(u => u.EMAIL == email && u.PASSWORD == password);
                
            if (user != null)
            {
                if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
                {
                    token = authorizationHeader.Substring("Bearer ".Length).Trim();

                    if (ValidateJwtToken(token))
                    {
                        return Ok(new { has_data = true, token = token, message = "Token is valid.", list = user });
                    }
                    else
                    {
                        var newToken = GenerateJwtToken(user);
                        return Ok(new { has_data = true, token = newToken, message = "Token generated successfully.", list= user });
                    }
                }
                else
                {
                    var newToken = GenerateJwtToken(user);
                    return Ok(new { has_data = true, token = newToken, message = "Token generated successfully.", list = user });
                }
            }

            return Content(HttpStatusCode.Unauthorized, new { has_data = false, message = "Login Failed." });
        }

        private string GenerateJwtToken(Login user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tkn_key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.USER_NAME),
                new Claim(ClaimTypes.Email, user.EMAIL),
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString())
            }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tkn_key);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true; 
            }
            catch
            {
                return false; 
            }
        }

        // API endpoint to create a new user
        [HttpPost]
        [Route("api/create-user")]
        public async Task<IHttpActionResult> CreateUser(Login user)
        {
            //var authorizationHeader = HttpContext.Current.Request.Headers["Authorization"];
            //string token = null;

            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }

            bool emailDoesNotExist = await CheckEmail(user.EMAIL);

            if (!emailDoesNotExist)
            {
                return Ok(new { has_data = false, message = "Email already exists." });
            }

            await _loginRepository.AddAsync(user);
            await _loginRepository.SaveAsync();

            return Ok(new { has_data = true, message = "Successfully Registered." });

        }

        [HttpGet]
        public async Task<bool> CheckEmail(string email)
        {
            var exists = await _loginRepository.GetAllAsync();

            // Check if the email exists
            bool emailExists = exists.Any(u => u.EMAIL == email);

            return !emailExists; 
        }
    }
}
