using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using VinylAppApi.Shared.Models.AuthorizationModels;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace VinylAppApi.Authorization.AuthorizationManager
{
    public class JwtService : IAuthService
    {
        public string SecretKey { get; set; }
        private IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
            SecretKey = _config.GetSection("ServerCredentials").ToString();
        }

        public IEnumerable<Claim> GetTokenClaims(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token is null or empty");
            }

            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters();

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token,
                                                                                   tokenValidationParameters,
                                                                                   out SecurityToken validatedToken);

                return tokenValid.Claims;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsTokenValid(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("given token is null or empty");
            }

            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters();

            JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                ClaimsPrincipal tokenValid = jwtTokenHandler
                    .ValidateToken(token, tokenValidationParameters, out SecurityToken validatedtoken);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public string TokenGeneration(IAuthContainerModel model)
        {
            if(model == null || model.Claims == null || model.Claims.Length == 0)
            {
                throw new ArgumentException("arguments to create token are not valid.");
            }

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(model.Claims),
                //Issuer = "familyvinylapp",
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(model.ExpireMinutes)),
                SigningCredentials = new SigningCredentials(GetSymmetricSecurityKey(), model.SecurityAlgorithm)
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var securityJwtToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            string token = jwtSecurityTokenHandler.WriteToken(securityJwtToken);

            return token;
        }


        //Private methods that create the key and set validation parameters;

        private SecurityKey GetSymmetricSecurityKey()
        {
            byte[] symmetricKey = Encoding.UTF8.GetBytes(SecretKey);

            return new SymmetricSecurityKey(symmetricKey);
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = GetSymmetricSecurityKey()
            };
        }
    }
}
