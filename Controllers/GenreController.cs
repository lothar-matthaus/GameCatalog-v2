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

                return Ok(new MessageSuccess
                {
                    Message = $"O gênero de ID '{genre.GenreId}' foi atualizado.",
                    Content = genre,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageError
                {
                    Message = "Erro ao atualizar o gênero.",
                    Success = false,
                    ErrorMessage = ex.Message
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

                return Ok(new MessageSuccess
                {
                    Message = $"O gênero de ID '{id}' foi deletado.",
                    Content = null,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageError
                {
                    Message = "Erro ao deletar o gênero.",
                    Success = false,
                    ErrorMessage = ex.Message
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

                    return Ok(new MessageSuccess
                    {
                        Success = true,
                        Message = $"O gênero '{genre.Name}' foi salvo.",
                        Content = genre
                    });
                }
                catch (Exception ex)
                {
                    return BadRequest(new MessageError
                    {
                        Message = "Não foi possível salvar o gênero.",
                        ErrorMessage = ex.Message
                    });
                }
            }
            else
            {
                return BadRequest(new MessageError
                {
                    Message = "Os dados inseridos estão incorretos.",
                    ErrorMessage = ModelState.Count.ToString()
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
    }
}