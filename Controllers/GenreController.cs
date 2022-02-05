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
    public class GenreController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public GenreController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public IActionResult Patch(JsonUpdateGenre jsonUpdateGenre)
        {
            TryValidateModel(jsonUpdateGenre);

            try
            {
                Genre genre = new Genre
                {
                    GenreId = jsonUpdateGenre.GenreId,
                    Name = jsonUpdateGenre.Name
                };

                _unitOfWork.Genre.Update(genre);

                return Ok(new APIMessage
                {
                    Message = $"O gênero de ID '{genre.GenreId}' foi atualizado.",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new APIMessage
                {
                    Message = $"Erro ao atualizar o gênero. {ex.Message}",
                    Success = false
                });
            }
        }
        
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                _unitOfWork.Genre.Remove(id);

                return Ok(new APIMessage
                {
                    Message = $"O gênero de ID '{id}' foi deletado.",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new APIMessage
                {
                    Message = $"Erro ao deletar o gênero. {ex.Message}",
                    Success = false
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Post(JsonNewGenre jsonNewGenre)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Genre genre = new Genre
                    {
                        Name = jsonNewGenre.Name
                    };

                    genre.GenreId = _unitOfWork.Genre.Save(genre);

                    return Ok(new APIMessage
                    {
                        Success = true,
                        Message = $"O gênero '{genre.Name}' foi salvo."
                    });
                }
                catch (Exception ex)
                {
                    return BadRequest(new APIMessage
                    {
                        Success = false,
                        Message = $"Não foi possível salvar o gênero. {ex.Message}",
                    });
                }
            }
            else
            {
                return BadRequest(new APIMessage
                {
                    Success = false,
                    Message = "Os dados inseridos estão incorretos.",
                });
            }
        }

        [HttpPost("New/List")]
        [Authorize(Roles = "Admin")]
        public IActionResult Post(ICollection<JsonNewGenre> jsonNewGenreList)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    List<Genre> genreList = new List<Genre>();

                    foreach (JsonNewGenre genre in jsonNewGenreList)
                    {
                        genreList.Add(new Genre { Name = genre.Name });
                    }

                    _unitOfWork.Genre.Save(genreList);

                    return Ok(new APIMessage
                    {
                        Success = true,
                        Message = $"Os gêneros inseridos foram salvos com sucesso!"
                    });
                }
                catch (Exception ex)
                {
                    return BadRequest(new APIMessage
                    {
                        Success = false,
                        Message = $"Não foi possível salvar os gêneros. {ex.Message}",
                    });
                }
            }
            else
            {
                return BadRequest(new APIMessage
                {
                    Success = false,
                    Message = "Os dados inseridos estão incorretos.",
                });
            }
        }


        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                IEnumerable<Genre> genreList = _unitOfWork.Genre.Get();

                return Ok(genreList);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Não foi possível carregar a lista de gêneros.",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                Genre genre = _unitOfWork.Genre.Get(id);

                if (genre == null)
                {
                    return Ok(new APIMessage
                    {
                        Success = true,
                        Message = $"Não foi encontrado um gênero com o Id '{id}' informado."
                    });
                }

                return Ok(genre);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Não foi possível carregar a lista de gêneros.",
                    Error = ex.Message
                });
            }
        }

        [HttpPost("List")]
        public IActionResult Get(ICollection<int> ids)
        {
            try
            {
                IEnumerable<Genre> genreList = _unitOfWork.Genre.Get(ids);

                if (genreList == null)
                {
                    return Ok(new APIMessage
                    {
                        Success = false,
                        Message = "Não foi encontrado nenhum gênero.",
                    });
                }

                return Ok(genreList);
            }
            catch (Exception ex)
            {
                return BadRequest(new APIMessage
                {
                    Success = false,
                    Message = $"Não foi possível carregar a lista de gêneros. {ex.Message}"
                });
            }
        }
    }
}