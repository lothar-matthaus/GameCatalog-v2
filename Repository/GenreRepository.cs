using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using GameCatalog.Data;
using GameCatalog.Entity.Models;
using GameCatalog.Repository.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GameCatalog.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private IDbSession _dbSession;

        public GenreRepository(IConfiguration configuration)
        {
            if (this._dbSession == null)
            {
                this._dbSession = new DbSession(configuration["connectionStrings:GameDataBase"]);
            }
        }
        public IEnumerable<Genre> Get()
        {
            string sqlCommand = "";

            try
            {
                sqlCommand = "SELECT * FROM Genre";

                IEnumerable<Genre> genreList = _dbSession.Connection.Query<Genre>(sqlCommand);

                return genreList;
            }
            catch (Exception ex)
            {
                _dbSession.Dispose();
                throw new Exception($"Erro ao coletar os gêneros no sistema. {ex.Message}"); ;
            }
        }

        public Genre Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Genre> Get(ICollection<int> genre)
        {
            string sqlCommand = "";

            try
            {
                sqlCommand = "SELECT * FROM Genre " +
                             "WHERE [GenreId] IN @Genre";

                IEnumerable<Genre> genreList = _dbSession.Connection.Query<Genre>(sqlCommand, new { Genre = genre });

                return genreList;
            }
            catch (Exception)
            {
                _dbSession.Rollback();
                throw;
            }
        }

        public int Remove(int id)
        {
            string sqlCommand = "DELETE FROM Genre " +
                                    "WHERE [GenreId] = @GenreId";

            try
            {
                _dbSession.BeginTransaction();

                int rows = _dbSession.Connection.Execute(sqlCommand, new { GenreId = id });

                if (rows == 0)
                    throw new Exception($"O ID '{id}' informado não foi encontrado na base de dados.");


                _dbSession.Commit();

                return rows;
            }
            catch (Exception)
            {
                _dbSession.Rollback();
                throw;
            }
        }

        public int Save(Genre genre)
        {
            string sqlCommand = "INSERT INTO Genre " +
                                "([Name])" +
                                "VALUES" +
                                "(@Name)" +
                                "returning [GenreId]";

            try
            {
                _dbSession.BeginTransaction();

                int genreId = _dbSession.Connection.QuerySingle<int>(sqlCommand, genre);

                _dbSession.Commit();

                return genreId;
            }
            catch (Exception)
            {
                _dbSession.Rollback();
                _dbSession.Dispose();

                throw;
            }
        }

        public int Update(Genre genre)
        {
            string sqlCommand = "UPDATE Genre " +
                                "SET [Name] = @Name " +
                                "WHERE [GenreId] = @GenreId";

            try
            {
                _dbSession.BeginTransaction();

                int rows = _dbSession.Connection.Execute(sqlCommand, genre);

                if (rows == 0)
                    throw new Exception($"O ID '{genre.GenreId}' informado não foi encontrado na base de dados.");

                _dbSession.Commit();

                return rows;
            }
            catch (Exception)
            {
                _dbSession.Rollback();
                throw;
            }
        }
    }
}