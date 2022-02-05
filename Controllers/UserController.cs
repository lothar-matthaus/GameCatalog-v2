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

namespace GameCatalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UserController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpPost("SignIn")]
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
                        return Ok(new APIMessage
                        {
                            Message = tokenService.Generate(user),
                            Success = true,
                        });
                    }
                    else
                    {
                        return Ok(new APIMessage
                        {
                            Message = "Usuário ou senha inválidos.",
                            Success = false
                        });
                    }
                }
                else
                {
                    return Ok(new APIMessage
                    {
                        Message = "Usuário não cadastrado no sistema.",
                        Success = true
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new APIMessage
                {
                    Message = $"Erro ao fazer o login no sistema. {ex.Message}",
                    Success = false,
                });
            }

        }

        [HttpPatch]
        [Authorize(Roles = "Admin,User")]
        public IActionResult Patch(JsonUpdateUser jsonUpdateUser)
        {
            UserService userService = new UserService();

            try
            {
                string saltHashed = userService.Encrypt(Guid.NewGuid().ToString());
                jsonUpdateUser.Password = userService.Encrypt(saltHashed, jsonUpdateUser.Password, 256);

                User user = new User
                {
                    UserId = jsonUpdateUser.UserId,
                    Email = jsonUpdateUser.Email,
                    FullName = jsonUpdateUser.FullName,
                    UserRole = Enum.GetName(typeof(UserRole), jsonUpdateUser.UserRole),
                    Login = new Login
                    {
                        Email = jsonUpdateUser.Email,
                        Password = jsonUpdateUser.Password,
                        Salt = saltHashed
                    }
                };

                int id = _unitOfWork.User.Update(user);

                return Ok(new APIMessage
                {
                    Success = true,
                    Message = $"O usuário '{user.FullName}' foi atualizado.",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new APIMessage
                {
                    Success = false,
                    Message = $"Erro ao atualizar o usuário. {ex.Message}",
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

                return Ok(new APIMessage
                {
                    Success = true,
                    Message = $"O usuário '{user.FullName}' foi salvo.",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new APIMessage
                {
                    Success = false,
                    Message = $"Erro ao criar o usuário. {ex.Message}",
                });
            }
        }
    }
}
