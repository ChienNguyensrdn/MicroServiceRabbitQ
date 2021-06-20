using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace LeaveProcessService.DataAcessHelper
{
    public interface IOracleDBManager
    {
        Task<int> ExecuteNonQueryAsync();
        Task<DataTable> GetDataTableAsync();
        Task<DataTable> GetDataTableAsync(string strCommand);

        T GetParameter<T>(string parameterName);

        T GetParameterOrDefault<T>(string parameterName);

        void AddParameter(string name, OracleDbType type, int size, object value);

        void AddParameter(string name, OracleDbType type, object value);

        void AddParameter(string name, int size, object value);

        void AddParameter(string name, object value);

        void AddParameter(string name, OracleDbType type, ParameterDirection direction, object value);

        void AddParameter(string name, OracleDbType type, int size, ParameterDirection direction, object value);

        void ExecuteStore(string functionName, string storeName, CustomStoreParam parameterInput,
          ref string parameterOutput, ref string json, ref int errorCode, ref string errorString, bool IsNoCache = true);

        void ExecuteStoreToDataset(string functionName, string storeName, dynamic parameterInput, ref string parameterOutput, ref string json, ref int errorCode, ref string errorString, bool IsNoCache = false);

    }
}
