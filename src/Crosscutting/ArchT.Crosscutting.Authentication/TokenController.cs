using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace ArchT.Crosscutting.Authentication
{
    [ApiController, Route("[controller]")]
    public class TokenController : ControllerBase
    {
        [HttpPost, AllowAnonymous]
        public ActionResult<AccessTokensResponse> RequestToken([FromForm]LoginRequest request)
        {
            var identity = new ClaimsIdentity(
               new GenericIdentity(request.client_id, "UserId"),
               new[]
               {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.UniqueName, request.client_id)
               });

            var token = identity.GenerateSecurityToken(request.client_secret);

            return Ok(new AccessTokensResponse(token));
        }

        public class LoginRequest
        {
            [Required]
            public string grant_type { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public string refresh_token { get; set; }
            public string scope { get; set; }
            public string client_id { get; set; }
            public string client_secret { get; set; }
        }

        public class AccessTokensResponse
        {
            public AccessTokensResponse(JwtSecurityToken securityToken)
            {
                access_token = new JwtSecurityTokenHandler().WriteToken(securityToken);
                token_type = JwtBearerDefaults.AuthenticationScheme;
                expires_in = Math.Truncate((securityToken.ValidTo - DateTime.Now).TotalSeconds);
            }

            public string access_token { get; set; }
            public string refresh_token { get; set; }
            public string token_type { get; set; }
            public double expires_in { get; set; }
        }
    }
}
