using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeStoresApi.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BikeStoresApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BikeStoresApi.Models.Repositories;
using BikeStoresApi.Models.Contracts;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BikeStoresApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly IUserInfo _userInfoRepository;

        public TokenController(IConfiguration config, IUserInfo userInfoRepository)
        {
            _configuration = config;
            _userInfoRepository = userInfoRepository;
        }


        /// <summary>
        /// entrega un token valido
        /// </summary>
        /// <remarks>
        /// usuario valido
        /// {
        ///    "userId": 1,
        ///    "firstName": "BikeStore",
        ///    "lastName": "Admin",
        ///    "userName": "BikeStoreAdmin",
        ///    "email": "BikeStoreAdmin@abc.com",
        ///    "password": "$admin@1985",
        ///    "createdDate": "2021-12-13T14:18:34.16"
        ///}
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> Post(UserInfo _userData)
        {

            if (_userData != null && _userData.Email != null && _userData.Password != null)
            {
                var user = await _userInfoRepository.GetByEmailAndPass(_userData.Email, _userData.Password);

                if (user != null)
                {
                    string issuer = _configuration.GetSection("Jwt:Issuer").Value;
                    string audience = _configuration.GetSection("Jwt:Audience").Value;
                    string Jwtkey = _configuration.GetSection("Jwt:Key").Value; 

                    //create claims details based on the user information
                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", user.UserId.ToString()),
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName),
                    new Claim("UserName", user.UserName),
                    new Claim("Email", user.Email)
                   };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwtkey));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }


    }
}
