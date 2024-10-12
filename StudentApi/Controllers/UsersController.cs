using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentApi.Data;
using StudentApi.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController(JwtOptions jwtOptions, AppDbcontext dbcontext) : ControllerBase
    {
        [HttpPost("Register")]
        public ActionResult Register()
        {

            return Ok();

        }


        [HttpPost("Login")]
        [Route("auth")]

        public ActionResult<string> Login(AuthenticatinRequest request)
        {
            var user = dbcontext.Users.FirstOrDefault(x =>
                                    x.UserName == request.userName &&
                                    x.Password == request.password);

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
                    new Claim(ClaimTypes.Name, user.UserName)
                })

            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return Ok(accessToken);
        }
    }
}
