using aspnet_b8_Md_Faisal_Mahmud.src.Assignment_4.Object_Relational_Mapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Object_Relational_Mapper
{
    public class AdoNetUtility : IDisposable
    {
        private readonly DbConnection _connection;
       

        public AdoNetUtility(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            
        }
        public List<DbCommand> WriteOperation(string sql, IList<DbParameter> parameters, List<DbCommand> commands)
        {
            using var command = CreateCommand(sql, parameters);
            
            commands.Add(command);

            return commands;
            

        }
        public int WriteFinal(List<DbCommand> commands)
        {
            int effected = 0;
            using (_connection)
            {
                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    _connection.Open();
                }

                    foreach (var command in commands)
                    {
                        command.Connection = _connection;
                        effected += command.ExecuteNonQuery();
                    }

            }

            return effected;
        }


        public void Dispose()
        {
            _connection.Dispose();
        }

        public IList<Dictionary<string, object>> ReadOperation(string sql,
            IList<DbParameter> parameters, bool isStoredProcedure)
        {
            using var command = CreateCommand(sql, parameters);

            if (isStoredProcedure)
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
            }

            var reader = command.ExecuteReader();

            var rows = new List<Dictionary<string, object>>();

            while (reader.Read())
            {
                var row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    object columnValue = reader.GetValue(i);

                    row.Add(columnName, columnValue);
                }

                rows.Add(row);
            }

            return rows;
        }

        private DbCommand CreateCommand(string sql, IList<DbParameter> parameters)
        {
            DbCommand command = new SqlCommand(sql, _connection as SqlConnection);

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters.ToArray());
            }

            return command;
        }

       
    }

}
