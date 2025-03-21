﻿using BookManagementApi_GO.BusinessLogic;
using BookManagementApi_GO.Model.Entities;
using BookManagementApi_GO.Model.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookManagementApi_GO.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class AuthController ( IConfiguration configuration , IAuthService authService ) : ControllerBase
    {
        [HttpPost ("login")]
        public async Task<ActionResult<LoginResponseModel>> Login ( [FromBody] LoginModel loginModel )
        {
            var user = await authService.GetUserByLogin (loginModel.Username , loginModel.Password);

            if (user != null)
            {
                var token = GenerateJwtToken (user , isRefreshToken: false);
                var refreshToken = GenerateJwtToken (user , isRefreshToken: true);

                await authService.AddRefreshTokenModel (new RefreshTokenModel
                {
                    RefreshToken = refreshToken ,
                    UserID = user.Id
                });

                return Ok (new LoginResponseModel
                {
                    Token = token ,
                    RefreshToken = refreshToken ,
                    TokenExpired = DateTimeOffset.UtcNow.AddMinutes (30).ToUnixTimeSeconds () ,
                });
            }
            return Unauthorized (); // Return proper HTTP response
        }



        [HttpGet ("loginByRefeshToken")]
        public async Task<ActionResult<LoginResponseModel>> LoginByRefeshToken ( string refreshToken )
        {
            var refreshTokenModel = await authService.GetRefreshTokenTokenModel (refreshToken);
            if (refreshTokenModel == null)
            {
                return StatusCode (StatusCodes.Status400BadRequest);
            }

            var newToken = GenerateJwtToken (refreshTokenModel.User , isRefreshToken: false);
            var newRefreshToken = GenerateJwtToken (refreshTokenModel.User , isRefreshToken: true);

            await authService.AddRefreshTokenModel (new RefreshTokenModel
            {
                RefreshToken = refreshToken ,
                UserID = refreshTokenModel.ID
            });

            return new LoginResponseModel
            {
                Token = newToken ,
                TokenExpired = DateTimeOffset.UtcNow.AddMinutes (30).ToUnixTimeSeconds () ,
                RefreshToken = newRefreshToken ,
            };

        }


        private string GenerateJwtToken ( UserModel user , bool isRefreshToken )
        {
            var claims = new List<Claim> ()
            {
                new Claim(ClaimTypes.Name, user.Username),
            };
            claims.AddRange (user.UserRoles.Select (n => new Claim (ClaimTypes.Role , n.Role.RoleName)));

            string secret = configuration.GetValue<string> ($"Jwt:{(isRefreshToken ? "RefreshTokenSecret" : "Secret")}");
            var key = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (secret));
            var creds = new SigningCredentials (key , SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken (
                issuer: "doseHp" ,
                audience: "doseHp" ,
                claims: claims ,
                expires: DateTime.UtcNow.AddMinutes (isRefreshToken ? 24 * 60 : 30) ,
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler ().WriteToken (token);
        }
    }
}
