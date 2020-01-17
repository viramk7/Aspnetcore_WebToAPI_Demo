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
        private readonly ITokenFactory _tokenFactory;

        public AuthController(
            UserManager<AppUser> userManager,
            IJwtFactory jwtFactory,
            ITokenFactory tokenFactory)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenFactory;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginInputDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound();

            var isAuthenticated = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isAuthenticated)
                return StatusCode(StatusCodes.Status401Unauthorized);

            var refreshToken = _tokenFactory.GenerateToken();
            user.RefreshToken = refreshToken;

            await _userManager.UpdateAsync(user);

            var response = new LoginOutputDto(
                await _jwtFactory.GenerateEncodedToken(user.Id, user.UserName),
                refreshToken,
                true);

            return Ok(response);

        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken(RefreshTokenInputDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return NotFound();

            if (user.RefreshToken != dto.RefreshToken)
                return BadRequest("Invalid refresh token");

            var newToken = await _jwtFactory.GenerateEncodedToken(user.Id, user.UserName);
            var newRefreshToken = _tokenFactory.GenerateToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            var response = new LoginOutputDto(newToken, newRefreshToken, true);

            return Ok(response);

        }

    }
}