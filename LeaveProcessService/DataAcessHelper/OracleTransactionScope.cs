using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using System.Web;

namespace LeaveProcessService.DataAcessHelper
{
    public class OracleTransactionScope: IDisposable
    {
        private readonly string _connectionString = "";// ConfigurationManager.AppSettings["DbConnection"];

        public OracleConnection Connection { get; private set; }
        public OracleTransaction Transaction { get; private set; }

        public OracleTransactionScope()
        {
            BeginTransaction().Wait();
            //HttpContext.Current.Items["DbTransactionScope"] = this;
        }
        public OracleTransactionScope(IsolationLevel isolationLevel)
        {
            BeginTransaction(isolationLevel).Wait();
            //HttpContext.Current.Items["DbTransactionScope"] = this;
        }
        public void Commit()
        {
            Transaction.Commit();
        }

        public void RollBackTransaction()
        {
            Transaction.Rollback();
            this.Dispose();
        }

        public void Dispose()
        {
            //HttpContext.Current.Items.Remove("DbTransactionScope");
            Transaction.Dispose();
            Connection.Dispose();
        }
        #region private open connection and begin transaction
        private async Task BeginTransaction()
        {
            Connection = new OracleConnection(_connectionString);
            await Connection.OpenAsync();
            Transaction = Connection.BeginTransaction();
        }
        private async Task BeginTransaction(IsolationLevel isolationLevel)
        {
            Connection = new OracleConnection(_connectionString);
            await Connection.OpenAsync();
            Transaction = Connection.BeginTransaction(isolationLevel);
        }
        #endregion
    }
}
