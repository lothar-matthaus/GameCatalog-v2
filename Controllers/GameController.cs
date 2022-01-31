using System;
using System.Collections.Generic;
using Dapper;
using GameCatalog.Entity.Json;
using GameCatalog.Entity.Message;
using GameCatalog.Entity.Models;
using GameCatalog.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GameController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                _unitOfWork.Game.Remove(id);

                return Ok(new MessageSuccess
                {
                    Message = $"O jogo de ID '{id}' foi deletado.",
                    Id = id,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageError
                {
                    Message = "Erro ao deletar o jogo.",
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpPost]
        public IActionResult Post(JsonNewGame jsonNewGame)
        {
            TryValidateModel(jsonNewGame);

            try
            {
                Game game = new Game
                {
                    Title = jsonNewGame.Title,
                    Description = jsonNewGame.Description,
                    CoverUrl = jsonNewGame.CoverUrl,
                    ReleaseDate = jsonNewGame.ReleaseDate,
                    Genre = _unitOfWork.Genre.Get(jsonNewGame.Genre).AsList<Genre>()
                };

                int id = _unitOfWork.Game.Save(game);

                return Ok(new MessageSuccess
                {
                    Success = true,
                    Message = "O título foi cadastrado com sucesso.",
                    Id = id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageError
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Game> gameList = null;
            try
            {
                gameList = _unitOfWork.Game.Get();

                if (gameList == null)
                {
                    return Ok(new MessageError
                    {
                        Success = true,
                        Message = "Não foram encontrados nenhum título na base de dados."
                    });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
            return Ok(gameList);
        }
    }
}