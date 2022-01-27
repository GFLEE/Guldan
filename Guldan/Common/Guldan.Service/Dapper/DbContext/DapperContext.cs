using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldan.Service.Dapper.DbContext
{
    public class DapperContext : DisposableObject, IUnitOfWork, IDapperContext
    {
        private readonly ConnectionStringSettings _connectionSeting =
           ConfigurationManager.ConnectionStrings["BudisengFirstDemoInfo"];
        public Guid Id { private set; get; }
        public IDbConnection Conn { private set; get; }
        public IDbTransaction Tran { private set; get; }

        public DapperContext()
        {
            Id = new Guid();
            InitConnection();
        }

        public DapperContext(ConnectionStringSettings connectionSeting)
        {
            // TODO: Complete member initialization
            this._connectionSeting = connectionSeting;
            InitConnection();
        }


        public void InitConnection()
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(this._connectionSeting.ProviderName);
            this.Conn = dbfactory.CreateConnection();
            if (Conn != null) this.Conn.ConnectionString = this._connectionSeting.ConnectionString;
        }

        private bool _committed = true;
        private readonly object _sync = new object();

        public bool Committed
        {
            set { _committed = value; }
            get { return _committed; }
        }

        public void UseTransaction(Action p)
        {
            try
            {
                this.BeginTran();

                p.Invoke();

                this.Commit();
            }
            catch (Exception ex)
            {
                this.Rollback();
                throw ex;
            }
            finally
            {
                this.Dispose(true);
            }

        }

        public void BeginTran()
        {
            this.Tran = this.Conn.BeginTransaction();
            this.Committed = false;
        }

        public void Commit()
        {
            if (Committed) return;
            lock (_sync)
            {
                this.Tran.Rollback();
                this._committed = true;
            }
        }

        public void Rollback()
        {
            if (Committed) return;
            lock (_sync)
            {
                this.Tran.Rollback();
                this._committed = true;
            }
        }



        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            if (this.Conn.State != ConnectionState.Open)
            {
                return;
            }
            Commit();
            this.Conn.Close();
            this.Conn.Dispose();
        }

        public virtual T Add<T>(T entity)
        {
            return default(T);
        }

        ~DapperContext()
        {
            Dispose(false);
        }
    }
}
