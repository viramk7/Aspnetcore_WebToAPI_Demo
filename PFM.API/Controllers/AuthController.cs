using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PFM.API.Auth;
using PFM.API.Models.Entities;
using PFM.Models.InputDtos;
using PFM.Models.OutputDtos;

namespace PFM.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtFactory _jwtFactory;

        public AuthController(UserManager<AppUser> userManager, IJwtFactory jwtFactory)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginInputDto model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound();

            var isAuthenticated = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isAuthenticated)
                return StatusCode(StatusCodes.Status401Unauthorized);

            var response = new LoginOutputDto(
                await _jwtFactory.GenerateEncodedToken(user.Id, user.UserName),
                Guid.NewGuid().ToString().Replace("-", string.Empty),
                true);

            return Ok(response);

        }

    }
}