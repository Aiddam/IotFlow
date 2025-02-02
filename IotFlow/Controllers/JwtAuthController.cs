﻿using IotFlow.Abstractions.Interfaces.Services;
using IotFlow.Models.Constants;
using IotFlow.Models.DTO.JWT;
using Microsoft.AspNetCore.Mvc;

namespace IotFlow.Controllers
{
    public class JwtAuthController : BaseController
    {
        private readonly IUserService<JwtUserDto, RegisterUser, LoginUser, RefreshUser> _userService;

        public JwtAuthController(IUserService<JwtUserDto, RegisterUser, LoginUser, RefreshUser> userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<CreatedResult> RegisterUserAsync([FromBody] RegisterUser userModel)
        {
            JwtUserDto user = await _userService.RegisterNewUserAsync(userModel);

            JwtOnlyTokenDto result = ProcessUser(user);

            return Created(string.Empty, result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<IUserDto>> LoginUserAsync([FromBody] LoginUser userModel)
        {
            JwtUserDto user = await _userService.LoginUserAsync(userModel);

            JwtOnlyTokenDto result = ProcessUser(user);

            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<IUserDto>> RefreshJwtTokenAsync()
        {
            string? refreshToken = Request.Cookies[CookieConstant.REFRESH_CODE_COOKIE_NAME];

            if (refreshToken == null)
            {
                throw new ArgumentException("There is no refresh token.");
            }

            RefreshUser refreshUserWithRefreshToken = new RefreshUser()
            {
                RefreshToken = refreshToken
            };

            JwtUserDto user = await _userService.RefreshUserAsync(refreshUserWithRefreshToken);

            JwtOnlyTokenDto result = ProcessUser(user);

            return Ok(result);
        }

        [HttpPost("user-exists")]
        public async Task<ActionResult> DoesNameOrEmailExist([FromBody] UserNameOrEmail userCredential)
        {
            bool doesNameOrEmailExist = await _userService.DoesNameOrEmailExist(userCredential.NameOrEmail);

            var response = new { checkStatus = doesNameOrEmailExist };

            return Ok(response);
        }

        private JwtOnlyTokenDto ProcessUser(JwtUserDto user)
        {
            Response.Cookies.Append(CookieConstant.REFRESH_CODE_COOKIE_NAME, user.RefreshToken, new CookieOptions()
            {
                HttpOnly = true,
            });

            return new JwtOnlyTokenDto
            {
                Token = user.Token
            };
        }
    }
}
