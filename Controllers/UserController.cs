using System;
using GameCatalog.Entity.Json;
using GameCatalog.Entity.Message;
using GameCatalog.Entity.Models;
using GameCatalog.Repository.Interfaces;
using GameCatalog.Services;
using GameCatalog.Services.Interfaces;
using GameCatalogv2.Entity.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GameCatalog.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UserController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public IActionResult Post(JsonUserLogin jsonUserLogin)
        {
            IUserService userService = new UserService();
            ITokenService tokenService = new TokenService(_configuration);

            try
            {
                User user = _unitOfWork.User.Get(jsonUserLogin.Email);
                
                if (user != null)
                {
                    jsonUserLogin.Password = userService.Encrypt(user.Login.Salt, jsonUserLogin.Password, 256);

                    if (jsonUserLogin.Email.Equals(user.Email) && jsonUserLogin.Password.Equals(user.Login.Password))
                    {
                        return Ok(new TokenMessage
                        {
                            Message = "Logado no sistema no sistema.",
                            Success = true,
                            Token = tokenService.Generate(user)
                        });
                    }
                    else
                    {
                        return Ok(new MessageSuccess
                        {
                            Content = jsonUserLogin,
                            Message = "Usuário ou senha inválidos.",
                            Success = true
                        });
                    }
                }
                else
                {
                    return Ok(new MessageSuccess
                    {
                        Message = "Usuário não cadastrado no sistema.",
                        Success = true
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new MessageError
                {
                    Message = "Erro ao fazer o login no sistema.",
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }

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
                    UserRole = Enum.GetName(typeof(UserRole), jsonNewUser.UserRole),
                    Login = new Login
                    {
                        Email = jsonNewUser.Email,
                        Password = jsonNewUser.Password,
                        Salt = saltHashed
                    }
                };

                int id = _unitOfWork.User.Save(user);

                return Ok(new MessageSuccess
                {
                    Success = true,
                    Message = $"O usuário '{user.FullName}' foi salvo.",
                   Content = user
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
