using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labs.NET.AuthenticationHandler.WebAPI.Controllers
{
    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet, Route("current")]
        public ActionResult GetCurrentUser()
        {
            var claims = User.Claims
                .Select(c => new { c.Type, c.Value });

            return Ok(claims);
        }
    }
}
