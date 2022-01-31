using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

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