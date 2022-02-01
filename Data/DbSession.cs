using System;
using System.Data;
using System.Data.SQLite;
using GameCatalog.Repository.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GameCatalog.Data
{
    public class DbSession : IDbSession
    {
        public IDbConnection Connection { get; }
        public IDbTransaction DbTransaction { get; set; }

        public DbSession(string connectionString)
        {
            try
            {
                this.Connection = new SQLiteConnection(connectionString);

                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }
            }
            catch (SQLiteException ex)
            {
                throw new Exception($"Não foi possível conectar ao banco de dados. {ex.Message}");
            }
        }

        public void Dispose()
        {
            this.Connection?.Dispose();
        }

        public void Commit()
        {
            this.DbTransaction?.Commit();
        }

        public void Rollback()
        {
            this.DbTransaction?.Rollback();
        }

        public void BeginTransaction()
        {
            this.DbTransaction = this.Connection?.BeginTransaction();
        }
    }
}