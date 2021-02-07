using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylAppApi.Authorization.AuthorizationManager;
using VinylAppApi.DataAccess.DbManager;

namespace VinylAppApi.Controllers
{
    [Route("v1/api/[controller]")]
    [Authorize]
    public class UsersController : Controller
    {
        private ILogger<UsersController> _logger;
        private readonly IDbAccess _dbAccess;
        private IAuthService _test;

        public UsersController(ILogger<UsersController> logger, IDbAccess dbAccess, IAuthService test)
        {
            _logger = logger;
            _dbAccess = dbAccess;
            _test = test;
        }

        [HttpGet]
        public string GetUsersById()
        {
            var item = HttpContext.Request.Headers["Authorization"];

            var authHeader = item.ToString()["Bearer ".Length..].Trim();

            var result = _test.GetTokenClaims(authHeader).ToList().First().Subject.Claims.First().Value;

            _dbAccess.QueryUser(result, "");

            return result;
        }
    }
}
