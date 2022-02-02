using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using GameCatalog.Data;
using GameCatalog.Entity.Models;
using GameCatalog.Repository.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GameCatalog.Repository
{
    public class GameRepository : IGameRepository
    {
        private IDbSession _dbSession;

        public GameRepository(IConfiguration configuration)
        {
            if (this._dbSession == null)
            {
                this._dbSession = new DbSession(configuration["connectionStrings:GameDataBase"]);
            }
        }

        public IEnumerable<Game> Get()
        {
            string sqlCommand = "SELECT [GA].*, [GE].* FROM Game AS [GA] " +
                                    "INNER JOIN GameGenre AS [GG] " +
                                        "ON [GA].GameId = [GG].GameId " +
                                    "INNER JOIN Genre AS [GE] " +
                                        "ON [GG].GenreId = [GE].GenreId ";

            try
            {
                IEnumerable<Game> gameList = _dbSession.Connection.Query<Game, Genre, Game>(sqlCommand, (game, genre) =>
                {
                    game.Genre = new List<Genre>();
                    game.Genre.Add(genre);

                    return game;
                },
                splitOn: "GenreId");

                IEnumerable<Game> result = gameList.GroupBy(game => game.GameId).Select(g =>
                {
                    Game groupedGameList = g.First();
                    groupedGameList.Genre = g.Select(game => game.Genre.Single()).ToList();
                    return groupedGameList;
                });

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao coletar a lista de jogos do sistema. {ex.Message}");
            }
        }

        public Game Get(int id)
        {
            string sqlCommand = "";

            try
            {
                sqlCommand = "SELECT * FROM Game " +
                             "WHERE [GameId] = @GameId";

                Game game = _dbSession.Connection.QuerySingleOrDefault<Game>(sqlCommand, new { GameId = id });

                return game;
            }
            catch (Exception)
            {
                _dbSession.Rollback();
                throw;
            }
        }

        public int Remove(int id)
        {
            string sqlCommand = "DELETE FROM Game " +
                                    "WHERE [GameId] = @GameId";

            try
            {
                _dbSession.BeginTransaction();

                int rows = _dbSession.Connection.Execute(sqlCommand, new { GameId = id });

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

        public int Save(Game game)
        {
            string sqlCommand = "";

            try
            {
                _dbSession.BeginTransaction();

                sqlCommand = "INSERT INTO Game" +
                             "([Title], [Description], [CoverUrl], [ReleaseDate])" +
                             "VALUES" +
                             "(@Title, @Description, @CoverUrl, @ReleaseDate)" +
                             "RETURNING [GameId];";

                int gameId = _dbSession.Connection.QuerySingleOrDefault<int>(sqlCommand, game, _dbSession.DbTransaction);

                sqlCommand = "INSERT INTO GameGenre" +
                             "([GameId], [GenreId])" +
                             "VALUES" +
                             "(@GameId, @GenreId)";

                foreach (Genre genre in game.Genre)
                {
                    _dbSession.Connection.QuerySingleOrDefault(sqlCommand, new { GameId = gameId, GenreId = genre.GenreId }, _dbSession.DbTransaction);
                }

                _dbSession.Commit();

                return gameId;
            }
            catch (Exception)
            {
                _dbSession.Rollback();
                throw;
            }

        }

        public int Update(Game game)
        {
            string sqlCommand = "";

            try
            {
                sqlCommand = "UPDATE Game " +
                             "SET [Title] = @Title, " +
                                 "[Description] = @Description, " +
                                 "[CoverUrl] = @CoverUrl, " +
                                 "[ReleaseDate] = @ReleaseDate " +
                             "WHERE [GameId] = @GameId";

                _dbSession.BeginTransaction();

                int rows = _dbSession.Connection.Execute(sqlCommand, game, _dbSession.DbTransaction);

                if (rows == 0)
                    throw new Exception($"O ID '{game.GameId}' informado não foi encontrado na base de dados.");

                sqlCommand = "DELETE FROM GameGenre " +
                                "WHERE [GameId] = @GameId";

                rows = _dbSession.Connection.Execute(sqlCommand, new { GameId = game.GameId }, _dbSession.DbTransaction);

                if (rows == 0)
                    throw new Exception($"O ID '{game.GameId}' informado não coincide com os gêneros.");


                sqlCommand = "INSERT INTO GameGenre " +
                             "([GameId], [GenreId]) " +
                             "VALUES " +
                             "(@GameId, @GenreId)";

                foreach (Genre genre in game.Genre)
                {
                    _dbSession.Connection.QueryFirstOrDefault(sqlCommand, new { GameId = game.GameId, GenreId = genre.GenreId }, _dbSession.DbTransaction);
                }

                _dbSession.Commit();

                return game.GameId.Value;

            }
            catch (Exception)
            {
                _dbSession.Rollback();
                throw;
            }
        }
    }
}