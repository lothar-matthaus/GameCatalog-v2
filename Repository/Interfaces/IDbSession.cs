using System.Data;

namespace GameCatalog.Repository.Interfaces
{
    public interface IDbSession
    {
        public IDbConnection Connection { get; }
        public IDbTransaction DbTransaction { get; set; }

        void Commit();
        void Rollback();
        void Dispose();
        void BeginTransaction();
    }
}