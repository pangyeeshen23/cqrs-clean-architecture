using Application.Handler;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Dtos;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController
    {
        IUserHandler _userHandler;
        public UserController(IUserHandler messageHandler)
        {
            _userHandler = messageHandler;
        }

        [HttpGet]
        public async Task<List<User>> GetAll()
        {
            List<User> user = await _userHandler.GetUsers();
            return user;
        }
    }
}
