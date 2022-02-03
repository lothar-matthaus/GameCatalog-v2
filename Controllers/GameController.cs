using System;
using System.Collections.Generic;
using Dapper;
using GameCatalog.Entity.Json;
using GameCatalog.Entity.Message;
using GameCatalog.Entity.Models;
using GameCatalog.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public IActionResult Patch(JsonUpdateGame jsonUpdateGame)
        {
            TryValidateModel(jsonUpdateGame);

            try
            {
                List<Genre> genres = new List<Genre>();

                foreach (int genreId in jsonUpdateGame.Genres)
                {
                    genres.Add(new Genre { GenreId = genreId });
                }

                Game game = new()
                {
                    GameId = jsonUpdateGame.GameId,
                    Title = jsonUpdateGame.Title,
                    CoverUrl = jsonUpdateGame.CoverUrl,
                    Description = jsonUpdateGame.Description,
                    Genre = genres
                };

                _unitOfWork.Game.Update(game);

                return Ok(new MessageSuccess
                {
                    Message = $"O jogo '{game.Title}' foi atualizado.",
                    Content = null,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageError
                {
                    Message = "Erro ao atualizar o jogo.",
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                _unitOfWork.Game.Remove(id);

                return Ok(new MessageSuccess
                {
                    Message = $"O jogo de ID '{id}' foi deletado.",
                    Content = null,
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
        [Authorize(Roles = "Admin")]
        public IActionResult Post(JsonNewGame jsonNewGame)
        {
            TryValidateModel(jsonNewGame);

            try
            {
                List<Genre> genres = new List<Genre>();

                foreach (int genreId in jsonNewGame.Genres)
                {
                    genres.Add(new Genre { GenreId = genreId });
                }

                Game game = new Game
                {
                    Title = jsonNewGame.Title,
                    Description = jsonNewGame.Description,
                    CoverUrl = jsonNewGame.CoverUrl,
                    ReleaseDate = jsonNewGame.ReleaseDate,
                    Genre = genres
                };


                int id = _unitOfWork.Game.Save(game);

                return Ok(new MessageSuccess
                {
                    Success = true,
                    Message = $"O título '{game.Title}' foi cadastrado com sucesso.",
                    Content = null
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

                if (gameList != null)
                {
                    return Ok(gameList);
                }
                else
                {
                    return Ok(new MessageError
                    {
                        Success = true,
                        Message = "Não foram encontrados nenhum título na base de dados.",
                        ErrorMessage = "Lista vazia."
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageError
                {
                    Success = true,
                    Message = "Erro ao coletar os jogos salvos no sistema.",
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                Game game = _unitOfWork.Game.Get(id);

                if (game != null)
                {
                    return Ok(game);
                }
                else
                {
                    return Ok(new MessageError
                    {
                        Success = true,
                        Message = "Não foi encontrado nenhum título.",
                        ErrorMessage = "O ID informado não existe na base de dados."
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageError
                {
                    Success = true,
                    Message = "Erro ao coletar os jogos salvos no sistema.",
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}