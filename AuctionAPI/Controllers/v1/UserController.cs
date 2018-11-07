using System;
using System.Web.Http;
using AuctionAPI.Dtos;
using AuctionAPI.Dtos.User;
using AuctionAPI.Services;

namespace AuctionAPI.Controllers.v1 {
    [RoutePrefix("api/v1/user")]
    public class UserController : ApiController
    {
        private readonly UserService _userService = new UserService();

        [Route("{id}")]
        public IHttpActionResult Get(Guid id)
        {
            var user = _userService.GetUser(id);
            return Ok(user);
        }

        [HttpPost]
        [Route("register")]
        public IHttpActionResult RegisterUser(RegisterUserDto dto)
        {
            _userService.RegisterUser(dto);
            return Created(String.Empty, new ApiResponse());
        }

        [HttpPost]
        [Route("changepassword")]
        public IHttpActionResult ChangePassword(ChangePasswordDto dto) {
            _userService.ChangePassword(dto);
            return Created(String.Empty, new ApiResponse());
        }
    }
}
