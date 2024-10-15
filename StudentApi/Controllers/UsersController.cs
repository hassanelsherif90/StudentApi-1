using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentApi.Data;
using StudentApi.Data.Entities;
using StudentApi.Model;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(JwtOptions jwtOptions, AppDbcontext dbcontext) : ControllerBase
    {

        [HttpPost]
        [Route("Register")]

        public ActionResult Register(AuthenticationRequest request)
        {
            var user = new User();

            if (ModelState.IsValid)
            {
                user.UserName = request.userName;
                user.Password = request.password;

                dbcontext.Users.Add(user);
                dbcontext.SaveChanges();
                return Ok();
            }
            return BadRequest("Invalid User Or Password");

        }


        [HttpPost]
        [Route("Login")]
        public ActionResult Login(AuthenticationRequest request)
        {
            var user = dbcontext.Users.FirstOrDefault(x => x.UserName == request.userName && x.Password == request.password);

            if (user == null)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,


                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                    SecurityAlgorithms.HmacSha256
                ),

                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString() ),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),

                Expires = DateTime.UtcNow.AddHours(1)

            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return Ok(new
            {
                securityToken = accessToken,
                Expires = DateTime.UtcNow.AddHours(1)
            });
        }
    }
}

