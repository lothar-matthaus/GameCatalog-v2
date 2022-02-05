using System;
using System.Collections.Generic;
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

                return Ok(game);
            }
            catch (Exception ex)
            {
                return BadRequest(new APIMessage
                {
                    Message = $"Erro ao atualizar o jogo. {ex.Message}",
                    Success = false
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

                return Ok(new APIMessage
                {
                    Message = $"O jogo de ID '{id}' foi deletado.",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new APIMessage
                {
                    Message = $"Erro ao deletar o jogo. {ex.Message}",
                    Success = false
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

                return Ok(new APIMessage
                {
                    Success = true,
                    Message = $"O título '{game.Title}' foi cadastrado com sucesso.",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new APIMessage
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
                    return Ok(new APIMessage
                    {
                        Success = true,
                        Message = "Não foram encontrados nenhum título na base de dados."
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new APIMessage
                {
                    Success = false,
                    Message = $"Erro ao coletar os jogos salvos no sistema. {ex.Message}",
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
                    return Ok(new APIMessage
                    {
                        Success = true,
                        Message = "Não foi encontrado nenhum título."
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new APIMessage
                {
                    Success = false,
                    Message = $"Erro ao coletar os jogos salvos no sistema. {ex.Message}",
                });
            }
        }
    }
}