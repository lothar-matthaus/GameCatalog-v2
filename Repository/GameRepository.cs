using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using GameCatalog.Data;
using GameCatalog.Entity.Models;
using GameCatalog.Repository.Interfaces;

namespace GameCatalog.Repository
{
    public class GameRepository : IGameRepository
    {
        private IDbSession DbSession;

        public GameRepository()
        {
            if (this.DbSession == null)
            {
                this.DbSession = new DbSession();
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
                IEnumerable<Game> gameList = DbSession.Connection.Query<Game, Genre, Game>(sqlCommand, (Game, Genre) =>
                {
                    Game.Genre = new List<Genre>();
                    Game.Genre.Add(Genre);

                    return Game;
                },
                splitOn: "GenreId");

                IEnumerable<Game> result = gameList.GroupBy(p => p.GameId).Select(g =>
                {
                    var groupedGameList = g.First();
                    groupedGameList.Genre = g.Select(p => p.Genre.Single()).ToList();
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
            throw new NotImplementedException();
        }

        public int Remove(int id)
        {
            string sqlCommand = "DELETE FROM Game " +
                                    "WHERE [GameId] = @GameId";

            try
            {
                DbSession.BeginTransaction();

                int rows = DbSession.Connection.Execute(sqlCommand, new { GameId = id });

                if (rows == 0)
                    throw new Exception($"O ID '{id}' informado não foi encontrado na base de dados.");

                DbSession.Commit();

                return rows;
            }
            catch (Exception)
            {
                DbSession.Rollback();
                throw;
            }
        }

        public int Save(Game game)
        {
            string sqlCommand = "";

            try
            {
                DbSession.BeginTransaction();

                sqlCommand = "INSERT INTO Game" +
                             "([Title], [Description], [CoverUrl], [ReleaseDate])" +
                             "VALUES" +
                             "(@Title, @Description, @CoverUrl, @ReleaseDate)" +
                             "RETURNING [GameId];";

                int gameId = DbSession.Connection.QuerySingleOrDefault<int>(sqlCommand, game, DbSession.DbTransaction);

                sqlCommand = "INSERT INTO GameGenre" +
                             "([GameId], [GenreId])" +
                             "VALUES" +
                             "(@GameId, @GenreId)";

                foreach (Genre genre in game.Genre)
                {
                    DbSession.Connection.QuerySingleOrDefault(sqlCommand, new { GameId = gameId, GenreId = genre.GenreId }, DbSession.DbTransaction);
                }

                DbSession.Commit();

                return gameId;
            }
            catch (Exception ex)
            {
                DbSession.Rollback();
                throw new Exception($"Erro ao salvar o título no sistema. {ex.Message}");
            }

        }

        public int Update(Game game)
        {
            throw new NotImplementedException();
        }
    }
}