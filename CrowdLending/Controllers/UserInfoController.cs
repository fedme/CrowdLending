using AspNet.Security.OpenIdConnect.Primitives;
using CrowdLending.Models;
using CrowdLending.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdLending.Controllers
{
    [Route("/[controller]")]
    [Authorize]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserInfoController(IUserService userService)
        {
            _userService = userService;
        }

        // GET /userinfo
        [HttpGet(Name = nameof(UserInfo))]
        [ProducesResponseType(401)]
        public async Task<ActionResult<UserInfoResponse>> UserInfo()
        {
            var user = await _userService.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "The user does not exist."
                });
            }

            var response = new UserInfoResponse()
            {
                FamilyName = user.LastName,
                GivenName = user.FirstName,
                Subject = user.Id.ToString(),
                Email = user.Email
            };

            return Ok(response);
        }
    }
}
