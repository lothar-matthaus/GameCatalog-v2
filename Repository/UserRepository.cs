using System;
using System.Collections.Generic;
using Dapper;
using GameCatalog.Data;
using GameCatalog.Entity.Models;
using GameCatalog.Repository.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GameCatalog.Repository
{
    public class UserRepository : IUserRepository
    {
        private IDbSession DbSession;

        public UserRepository(IConfiguration configuration)
        {
            if (this.DbSession == null)
            {
                this.DbSession = new DbSession(configuration["connectionStrings:UserDataBase"]);
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

        public int Remove(int id)
        {
            throw new NotImplementedException();
        }

        public int Save(Login login)
        {
            string sqlCommand = "";

            try
            {
                DbSession.BeginTransaction();

                sqlCommand = "INSERT INTO Login" +
                             "([Email], [Password], [Salt])" +
                             "VALUES" +
                             "(@Email, @Password, @Salt) " +
                             "RETURNING [LoginId]";

                int loginId = DbSession.Connection.QuerySingle<int>(sqlCommand, login, DbSession.DbTransaction);

                DbSession.Commit();

                return loginId;
            }
            catch (Exception)
            {
                DbSession.Rollback();
                throw;
            }
        }

        public int Save(User user)
        {
            string sqlCommand = "";

            try
            {
                DbSession.BeginTransaction();

                sqlCommand = "INSERT INTO User" +
                             "([Email], [FullName], [UserRole])" +
                             "VALUES" +
                             "(@Email, @FullName, @UserRole) " +
                             "RETURNING [UserId]";

                int userId = DbSession.Connection.QuerySingle<int>(sqlCommand, user, DbSession.DbTransaction);

                DbSession.Commit();

                return userId;
            }
            catch (Exception)
            {
                DbSession.Rollback();
                throw;
            }
        }

        public int Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}