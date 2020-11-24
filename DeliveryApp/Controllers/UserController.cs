using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryApp.Controller.Models;
using DeliveryApp.Data;
using DeliveryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DeliveryApp.Services;
using System.Security.Claims;
using DeliveryApp.Controller.Model;
using System;

namespace DeliveryApp.Controller
{
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) 
        {
            _userService = userService;
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns>A newly created user</returns>
        /// <response code="201">Returns the newly created user</response>
        /// <response code="400">If One or more validation errors occur</response>   
        [Produces("application/json")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserResponse>> CreateUser([FromBody] UserRequest userRequest) 
        {
            if (ModelState.IsValid) 
            {
                var newUser = new User(
                    userRequest.Email,
                    userRequest.Name,
                    userRequest.Password,
                    userRequest.Telephone,
                    userRequest.Role);

                var createdUser = await _userService.CreateUser(newUser);
                return CreatedAtRoute("GetUserById", new {id = createdUser.Id}, new UserResponse(createdUser));
            }

            return BadRequest();
        }

        /// <summary>
        /// Gets an user given a id
        /// </summary>
        /// <param name="id">User's id</param>
        /// <returns>An user</returns>
        /// <response code="200">Returns an user</response>
        /// <response code="400">If One or more validation errors occur</response>   
        /// <response code="401">If user is not logged in</response>
        [Produces("application/json")]
        [HttpGet("{id}", Name = "GetUserById")]
        [Authorize]
        public async Task<ActionResult<UserResponse>> GetUserById([FromRoute] Guid id)
        {
            var user = await _userService.GetUserById(id);
            return Ok(new UserResponse(user));
        }

        /// <summary>
        /// An user can edit itself
        /// </summary>
        /// <param name="id">User's id</param>
        /// <param name="userUpdate">user update payload</param>
        /// <returns>An user</returns>
        /// <response code="200">Returns an user</response>
        /// <response code="400">If One or more validation errors occur</response>   
        /// <response code="401">If user is not logged in</response>
        [Produces("application/json")]
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<UserResponse>> UpdateUser([FromRoute] Guid id, [FromBody] UserUpdateRequest userUpdate)
        {
            if (ModelState.IsValid)
            {
                var updatedUser = await _userService.UpdateUser(id, userUpdate);
                return Ok(new UserResponse(updatedUser));
            }

            return BadRequest();
        }

    }
}