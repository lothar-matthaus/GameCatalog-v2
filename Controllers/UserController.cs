using System;
using GameCatalog.Entity.Json;
using GameCatalog.Entity.Message;
using GameCatalog.Entity.Models;
using GameCatalog.Repository.Interfaces;
using GameCatalog.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("Login")]
        public IActionResult Post(Login login)
        {
            UserService userService = new UserService();

            return Ok(null);
        }
        [HttpPost]
        public IActionResult Post(JsonNewUser jsonNewUser)
        {
            UserService userService = new UserService();

            try
            {
                string saltHashed = userService.Encrypt(Guid.NewGuid().ToString());
                jsonNewUser.Password = userService.Encrypt(saltHashed, jsonNewUser.Password, 256);

                User user = new User
                {
                    Email = jsonNewUser.Email,
                    FullName = jsonNewUser.FullName,
                    UserRole = jsonNewUser.UserRole
                };

                int id = _unitOfWork.User.Save(user);

                Login login = new Login
                {
                    Email = jsonNewUser.Email,
                    Password = jsonNewUser.Password,
                    Salt = saltHashed
                };

                _unitOfWork.User.Save(login);

                return Ok(new MessageSuccess
                {
                    Success = true,
                    Message = $"O usuário '{user.FullName}' foi salvo.",
                    Id = id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageError
                {
                    Success = false,
                    Message = "Erro ao criar o usuário.",
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}
