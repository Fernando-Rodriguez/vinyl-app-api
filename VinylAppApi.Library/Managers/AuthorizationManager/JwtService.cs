using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using VinylAppApi.Library.Models.AuthorizationModels;


namespace VinylAppApi.Library.AuthorizationManager
{
    public class JwtService : IAuthService
    {
        public string SecretKey { get; set; }

        public JwtService(string secretKey)
        {
            SecretKey = secretKey;
        }

        public IEnumerable<Claim> GetTokenClaims(string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token is null or empty");
            }

            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters();

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                ClaimsPrincipal tokenValid = jwtSecurityTokenHandler
                    .ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return tokenValid.Claims;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public bool IsTokenValid(string token)
        {
            if (String.IsNullOrEmpty(token))
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
            byte[] symmetricKey = Convert.FromBase64String(SecretKey);

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

        public JwtContainerModel GetJwtContainerModel(string name, string email)
        {
            return new JwtContainerModel
            {
                Claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Email, email)
                }
            };
        }
    }
}
