using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using GameCatalog.Data;
using GameCatalog.Entity.Models;
using GameCatalog.Repository.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GameCatalog.Repository
{
    public class UserRepository : IUserRepository
    {
        private IDbSession _dbSession;

        public UserRepository(IConfiguration configuration)
        {
            if (this._dbSession == null)
            {
                this._dbSession = new DbSession(configuration["connectionStrings:UserDataBase"]);
            }
        }
        public IEnumerable<User> Get()
        {
            throw new NotImplementedException();
        }

        public User Get(int id)
        {
            throw new NotImplementedException();
        }

        public User Get(string email)
        {
            string sqlCommand = "";

            try
            {
                sqlCommand = "SELECT * FROM [User] AS [U] " +
                    "INNER JOIN [Login] AS [L] " +
                        "ON [U].[UserId] = [L].UserId " +
                    $"WHERE [U].[Email] = @Email";

                User user = _dbSession.Connection.Query<User, Login, User>(sqlCommand, (user, login) =>
                  {
                      user.Login = login;

                      return user;
                  },
                 splitOn: "UserId", param: new { Email = email }).FirstOrDefault<User>();

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Remove(int id)
        {
            throw new NotImplementedException();
        }

        public int Save(User user)
        {
            string sqlCommand = "";

            try
            {
                _dbSession.BeginTransaction();

                sqlCommand = "INSERT INTO User" +
                             "([Email], [FullName], [UserRole])" +
                             "VALUES" +
                             "(@Email, @FullName, @UserRole) " +
                             "RETURNING [UserId]";

                int userId = _dbSession.Connection.QuerySingle<int>(sqlCommand, user, _dbSession.DbTransaction);

                sqlCommand = "INSERT INTO Login" +
                             "([UserId], [Email], [Password], [Salt])" +
                             "VALUES" +
                             "(@UserId, @Email, @Password, @Salt) ";

                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("UserId", userId, DbType.Int32, ParameterDirection.Input);
                parameters.Add("Email", user.Login.Email, DbType.String, ParameterDirection.Input);
                parameters.Add("Password", user.Login.Password, DbType.String, ParameterDirection.Input);
                parameters.Add("Salt", user.Login.Salt, DbType.String, ParameterDirection.Input);

                _dbSession.Connection.QuerySingleOrDefault(sqlCommand, parameters, _dbSession.DbTransaction);

                _dbSession.Commit();

                return userId;
            }
            catch (Exception)
            {
                _dbSession.Rollback();
                throw;
            }
        }

        public int Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}