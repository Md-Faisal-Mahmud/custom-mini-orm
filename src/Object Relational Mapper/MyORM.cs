using Object_Relational_Mapper;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.Text.RegularExpressions;

namespace aspnet_b8_Md_Faisal_Mahmud.src.Assignment_4.Object_Relational_Mapper
{

    public class MyORM<G, T>
    {
        private readonly string _connectionString;
        private readonly bool _isAutoIncremented;
        public List<DbCommand> commands = new List<DbCommand>();
        public MyORM(string connectionString)
        {
            _connectionString = connectionString;
            _isAutoIncremented = false; //typeof(G) == typeof(int);
        }

        #region Insert(T item)
        public void Insert(T item)
        {
            string fkNameParentIList = null;


            G? parentObjectId = default(G?);


            InsertRecursive(item, fkNameParentIList, parentObjectId);

            using (var adoNetUtility = new AdoNetUtility(_connectionString))
            {
                DbCommand command = commands.Last();
                string wrongFk = typeof(T).Name + "Id";
                if (command.CommandText.Contains(wrongFk))
                {
                    // Find the index of the parameter with the name matching wrongFk
                    int paramIndex = -1;
                    for (int i = 0; i < command.Parameters.Count; i++)
                    {
                        if (command.Parameters[i].ParameterName == "@" + wrongFk)
                        {
                            paramIndex = i;
                            break;
                        }
                    }

                    // Remove the parameter from the command's Parameters collection
                    if (paramIndex >= 0)
                    {
                        command.Parameters.RemoveAt(paramIndex);
                    }

                    // Remove the column from the command's CommandText
                    command.CommandText = command.CommandText.Replace("," + wrongFk, "");
                    command.CommandText = command.CommandText.Replace(",@" + wrongFk, "");
                }

                OneToOneFK(item);
                List<string> sortedTable = new List<string>();
                IEnumerable<PropertyInfo> properties;
                Type itemType = item.GetType();
                string tableName = itemType.Name;


                properties = itemType.GetProperties();
                sortedTable.Add(tableName);

                 SortTable(commands, item, ref sortedTable);


                sortedTable = sortedTable.Distinct().ToList();
                Dictionary<string, int> tableIndex = new Dictionary<string, int>();
                int currentIndex = 0;
                foreach (var table in sortedTable)
                {
                    if (!tableIndex.ContainsKey(table))
                    {
                        tableIndex.Add(table, currentIndex++);
                    }
                }

                // Sort the commands based on the table name
                List<DbCommand> sortedCommands = commands.OrderBy(cmd =>
                {
                    // Extract the table name from the command text
                    string tableName = Regex.Match(cmd.CommandText, @"INSERT INTO (\w+)").Groups[1].Value;

                    // Get the index of the table name from the dictionary
                    if (tableIndex.ContainsKey(tableName))
                    {
                        return tableIndex[tableName];
                    }
                    else
                    {
                        return int.MaxValue; // Put any unknown table names at the end
                    }
                }).ToList();


                adoNetUtility.WriteFinal(sortedCommands);
                Console.WriteLine("Successfully inserted in the database!");
            }
        }
        
        private void InsertRecursive(object item, string fkNameParentIList, G? parentObjectId)
        {
            Type itemType = item.GetType();
            string tableName = itemType.Name;
            IEnumerable<PropertyInfo> properties;
            if (_isAutoIncremented)
            {
                properties = itemType.GetProperties().Where(p => p.Name != "Id");
            }
            else
            {
                properties = itemType.GetProperties();
            }

            // Generate the SQL query
            (string sql, List<DbParameter> parameters) SqlAndParameters = GenerateSqlQuery(tableName, properties,
                                                                           item, fkNameParentIList, parentObjectId);

            using (var adoNetUtility = new AdoNetUtility(_connectionString))
            {


                foreach (PropertyInfo property in properties)
                {
                    if (property.PropertyType.IsGenericType &&
                        property.PropertyType.GetGenericTypeDefinition() == typeof(List<>) ||
                        property.PropertyType.IsArray)
                    {
           


                    }
                    else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                    {

                    }
                    else
                    {
                        string propertyName = property.Name;
                        var parameter = new SqlParameter($"@{propertyName}", property.GetValue(item) ?? DBNull.Value);
                        SqlAndParameters.parameters.Add(parameter);
                    }
                }

                adoNetUtility.WriteOperation(SqlAndParameters.sql, SqlAndParameters.parameters, commands);

            }
        }
        private void OneToOneFK(object item)
        {
            var propertyInfo = item.GetType().GetProperties();
            foreach (PropertyInfo property in propertyInfo)
            {
                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(List<>) ||
                    property.PropertyType.IsArray)
                {
                    var list = property.GetValue(item) as IList;


                    foreach (var listItem in list)
                    {

                        OneToOneFK(listItem);

                    }
                }
                else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {

                    var propertyValue = property.GetValue(item);
                    var oneToOneFK = property.Name + "Id";



                    foreach (var command in commands)
                    {

                        if (command.CommandText.Contains(" " + item.GetType().Name + " "))
                        {

                            int firstClosingParenIndex = command.CommandText.IndexOf(')');
                            command.CommandText = command.CommandText.Insert(firstClosingParenIndex, "," + oneToOneFK);

                            int lastClosingParenIndex = command.CommandText.LastIndexOf(')');
                            command.CommandText = command.CommandText.Insert(lastClosingParenIndex, ",@" + oneToOneFK);


                            var parameter = command.CreateParameter();
                            parameter.ParameterName = "@" + oneToOneFK;

                            if (propertyValue != null)
                            {
                                var idProperty = propertyValue.GetType().GetProperty("Id");
                                if (idProperty != null)
                                {
                                    parameter.Value = idProperty.GetValue(propertyValue);
                                }
                                else
                                {
                                }
                            }
                            else
                            {
                            }
                            command.Parameters.Add(parameter);

                        }
                    }




                    OneToOneFK(propertyValue);


                }
                else
                {


                }

            }


        }
        private (string, List<DbParameter>) GenerateSqlQuery(string tableName, IEnumerable<PropertyInfo> properties,
            object item, string fkNameParentIList, G? parentObjectId, string sqlColumns = null, string sqlValues = null)
        {

            List<DbParameter> parameters = new List<DbParameter>();
            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(List<>) ||
                    property.PropertyType.IsArray)
                {
                    var list = property.GetValue(item) as IList;


                    fkNameParentIList = item.GetType().Name + "Id";
                    parentObjectId = (dynamic)item.GetType().GetProperty("Id").GetValue(item);

                    foreach (var listItem in list)
                    {

                        InsertRecursive(listItem, fkNameParentIList, parentObjectId);

                    }
                }
                else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {


                    // If the property is a class, recursively call InsertRecursive method
                    var propertyValue = property.GetValue(item);
                    InsertRecursive(propertyValue, fkNameParentIList, parentObjectId);


                }
                else
                {
                    string propertyName = property.Name;
                    sqlColumns += $"{propertyName},";
                    sqlValues += $"@{propertyName},";

                    if (fkNameParentIList != null)
                    {
                        if (!sqlColumns.Contains(fkNameParentIList))
                        {
                            sqlColumns += $"{fkNameParentIList},";
                            sqlValues += $"@{fkNameParentIList},";
                            parameters.Add(new SqlParameter($"@{fkNameParentIList}", parentObjectId));
                        }

                    }

                }

            }


            // Remove the trailing commas
            sqlColumns = sqlColumns.TrimEnd(',');
            sqlValues = sqlValues.TrimEnd(',');


            string sql = $"INSERT INTO {tableName} ({sqlColumns}) VALUES ({sqlValues})";


            return (sql, parameters);

        }


        


        public void SortTable(List<DbCommand> commands, object item, ref List<string> sortedTable, bool flag = true)
        {
            IEnumerable<PropertyInfo> properties;
            Type itemType = item.GetType();
            string tableName = itemType.Name;

            properties = itemType.GetProperties();



            foreach (PropertyInfo property in properties)
            {
               
                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(List<>) ||
                    property.PropertyType.IsArray)
                {
                    var list = property.GetValue(item) as IList;
                    int index = sortedTable.IndexOf(item.GetType().Name);
                   
                    foreach (var listItem in list)
                    {
                        sortedTable.Insert(index + 1, listItem.GetType().Name);
                        SortTable(commands, listItem, ref sortedTable, false);

                    }
                }
                else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {

                    int index = sortedTable.IndexOf(item.GetType().Name);
                    sortedTable.Insert(index, property.PropertyType.Name);
                    SortTable(commands, property.GetValue(item), ref sortedTable, false);
                }
                else
                {

                }
            }
          
        }



        #endregion



        #region update(T item)


        public List<DbCommand> Updatecommands = new List<DbCommand>();
        public void Update(T item)
        {
            string fkNameParentIList = null;


            G? parentObjectId = default(G?);


            UpdateRecursive(item, fkNameParentIList, parentObjectId);

            using (var adoNetUtility = new AdoNetUtility(_connectionString))
            {
                DbCommand command = Updatecommands.Last();
                string wrongFk = typeof(T).Name + "Id";
                string wrongFKValue = "@" + typeof(T).Name + "Id";
                if (command.CommandText.Contains("," + wrongFk + "=" + wrongFKValue))
                {
                    int paramIndex = -1;
                    for (int i = 0; i < command.Parameters.Count; i++)
                    {
                        if (command.Parameters[i].ParameterName == "@" + wrongFk)
                        {
                            paramIndex = i;
                            break;
                        }
                    }

                    if (paramIndex >= 0)
                    {
                        command.Parameters.RemoveAt(paramIndex);
                    }

                    command.CommandText = command.CommandText.Replace("," + wrongFk + "=" + wrongFKValue, "");

                }




                UpdateOneToOneFK(item);
                List<string> UpdatesortedTable = new List<string>();
                IEnumerable<PropertyInfo> properties;
                Type itemType = item.GetType();
                string tableName = itemType.Name;


                properties = itemType.GetProperties();
                UpdatesortedTable.Add(tableName);

                UpdateSortTable(Updatecommands, item, ref UpdatesortedTable);


                UpdatesortedTable = UpdatesortedTable.Distinct().ToList();
                Dictionary<string, int> tableIndex = new Dictionary<string, int>();
                int currentIndex = 0;
                foreach (var table in UpdatesortedTable)
                {
                    if (!tableIndex.ContainsKey(table))
                    {
                        tableIndex.Add(table, currentIndex++);
                    }
                }

                List<DbCommand> sortedCommands = Updatecommands.OrderBy(cmd =>
                {
                    string tableName = Regex.Match(cmd.CommandText, @"INSERT INTO (\w+)").Groups[1].Value;

                    if (tableIndex.ContainsKey(tableName))
                    {
                        return tableIndex[tableName];
                    }
                    else
                    {
                        return int.MaxValue; 
                    }
                }).ToList();


                adoNetUtility.WriteFinal(sortedCommands);
            }

        }

        #region private helper methods
        private void UpdateRecursive(object item, string fkNameParentIList, G? parentObjectId)
        {

            
            if (item != null)
            {
                Type itemType = item.GetType();
                string tableName = itemType.Name;
                IEnumerable<PropertyInfo> properties;
                if (_isAutoIncremented)
                {
                    properties = itemType.GetProperties().Where(p => p.Name != "Id");
                }
                else
                {
                    properties = itemType.GetProperties();
                }

                // Generate the SQL query
                (string sql, List<DbParameter> parameters) UpdateSqlAndParameters = UpdateGenerateSqlQuery(tableName, properties,
                                                                               item, fkNameParentIList, parentObjectId);

                using (var adoNetUtility = new AdoNetUtility(_connectionString))
                {


                    foreach (PropertyInfo property in properties)
                    {
                        if (property.PropertyType.IsGenericType &&
                            property.PropertyType.GetGenericTypeDefinition() == typeof(List<>) ||
                            property.PropertyType.IsArray)
                        {


                        }
                        else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                        {

                        }
                        else
                        {
                            // Create the parameter for the property
                            string propertyName = property.Name;
                            var parameter = new SqlParameter($"@{propertyName}", property.GetValue(item) ?? DBNull.Value);
                            UpdateSqlAndParameters.parameters.Add(parameter);
                        }
                    }

                    adoNetUtility.WriteOperation(UpdateSqlAndParameters.sql, UpdateSqlAndParameters.parameters, Updatecommands);

                }
            }
           
        }
        private void UpdateOneToOneFK(object item)
        {
            if (item != null)
            {
                var propertyInfo = item.GetType().GetProperties();
                foreach (PropertyInfo property in propertyInfo)
                {
                    if (property.PropertyType.IsGenericType &&
                        property.PropertyType.GetGenericTypeDefinition() == typeof(List<>) ||
                        property.PropertyType.IsArray)
                    {
                        var list = property.GetValue(item) as IList;


                        foreach (var listItem in list)
                        {

                            UpdateOneToOneFK(listItem);

                        }
                    }
                    else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                    {


                        var propertyValue = property.GetValue(item);
                        var oneToOneFK = property.Name + "Id";



                        foreach (var command in Updatecommands)
                        {

                            if (command.CommandText.Contains(" " + item.GetType().Name + " "))
                            {

                                int firstClosingParenIndex = command.CommandText.IndexOf("SET");
                                command.CommandText = command.CommandText.Insert(firstClosingParenIndex + 3, " " + oneToOneFK + "=@" + oneToOneFK + ",");

                                var parameter = command.CreateParameter();
                                parameter.ParameterName = "@" + oneToOneFK;

                                if (propertyValue != null)
                                {
                                    var idProperty = propertyValue.GetType().GetProperty("Id");
                                    if (idProperty != null)
                                    {
                                        parameter.Value = idProperty.GetValue(propertyValue);
                                    }
                                    else
                                    {
                                        // handle case where object doesn't have an "Id" property
                                    }
                                }
                                else
                                {
                                    // handle case where propertyValue is null
                                }
                                command.Parameters.Add(parameter);

                            }
                        }
                        UpdateOneToOneFK(propertyValue);
                    }
                    else
                    {


                    }

                }
            }

          


        }
        private (string, List<DbParameter>) UpdateGenerateSqlQuery(string tableName, IEnumerable<PropertyInfo> properties,
            object item, string fkNameParentIList, G? parentObjectId, string sqlColumns = null, string sqlValues = null)
        {

            List<DbParameter> parameters = new List<DbParameter>();
           
            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(List<>) ||
                    property.PropertyType.IsArray)
                {
                    var list = property.GetValue(item) as IList;


                    fkNameParentIList = item.GetType().Name + "Id";
                    parentObjectId = (dynamic)item.GetType().GetProperty("Id").GetValue(item);

                    foreach (var listItem in list)
                    {

                        UpdateRecursive(listItem, fkNameParentIList, parentObjectId);

                    }
                }
                else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {


                    var propertyValue = property.GetValue(item);
                    UpdateRecursive(propertyValue, fkNameParentIList, parentObjectId);


                }
                else
                {
                    // Generate column and value for the property
                    string propertyName = property.Name;
                    var propertyValue= property.GetValue(item);
                    if (propertyValue != null)
                    {
                        sqlColumns += $"{propertyName},";
                        sqlValues += $"@{propertyName},";

                        if (fkNameParentIList != null)
                        {
                            if (!sqlColumns.Contains(fkNameParentIList))
                            {
                                sqlColumns += $"{fkNameParentIList},";
                                sqlValues += $"@{fkNameParentIList},";
                                parameters.Add(new SqlParameter($"@{fkNameParentIList}", parentObjectId));
                            }

                        }
                    }
                    

                }

            }


            // Remove the trailing commas
            sqlColumns = sqlColumns.TrimEnd(',');
            sqlValues = sqlValues.TrimEnd(',');

            var sqlC = sqlColumns.Split(",").Where(c => c != "Id").ToList(); // exclude Id column
            var sqlV = sqlValues.Split(",").Where(c => c != "@Id").ToList();

            var updateSql = "";
            for (int i = 0; i < sqlC.Count; i++)
            {
                if (i > 0) updateSql += ",";
                updateSql += $"{sqlC[i]}={sqlV[i]}";
            }


            // Generate the SQL query

            string sql = $"UPDATE {tableName} SET {updateSql} WHERE Id=@Id";


            return (sql, parameters);

        }


        #endregion


        public void UpdateSortTable(List<DbCommand> Updatecommands, object item, ref List<string> UpdatesortedTable, bool flag = true)
        {
            if (item != null)
            {
                IEnumerable<PropertyInfo> properties;
                Type itemType = item.GetType();
                string tableName = itemType.Name;

                properties = itemType.GetProperties();



                foreach (PropertyInfo property in properties)
                {

                    if (property.PropertyType.IsGenericType &&
                        property.PropertyType.GetGenericTypeDefinition() == typeof(List<>) ||
                        property.PropertyType.IsArray)
                    {
                        var list = property.GetValue(item) as IList;
                        int index = UpdatesortedTable.IndexOf(item.GetType().Name);

                        foreach (var listItem in list)
                        {
                            UpdatesortedTable.Insert(index + 1, listItem.GetType().Name);
                            UpdateSortTable(Updatecommands, listItem, ref UpdatesortedTable, false);

                        }
                    }
                    else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                    {

                        int index = UpdatesortedTable.IndexOf(item.GetType().Name);
                        UpdatesortedTable.Insert(index, property.PropertyType.Name);
                        UpdateSortTable(Updatecommands, property.GetValue(item), ref UpdatesortedTable, false);
                    }
                    else
                    {

                    }
                }
            }
           

        }

        #endregion




        #region Delete(T item)

        public List<DbCommand> deleteCommands = new List<DbCommand>();
        public void Delete(T item)
        {
            string fkNameParentIList = null;


            G? parentObjectId = default(G?);


            DeleteRecursive(item, fkNameParentIList, parentObjectId);

            using (var adoNetUtility = new AdoNetUtility(_connectionString))
            {
                DbCommand command = deleteCommands.Last();
                string wrongFk = typeof(T).Name + "Id";
                string wrongFKValue = "@" + typeof(T).Name + "Id";
                if (command.CommandText.Contains("," + wrongFk + "=" + wrongFKValue))
                {
                    int paramIndex = -1;
                    for (int i = 0; i < command.Parameters.Count; i++)
                    {
                        if (command.Parameters[i].ParameterName == "@" + wrongFk)
                        {
                            paramIndex = i;
                            break;
                        }
                    }

                    if (paramIndex >= 0)
                    {
                        command.Parameters.RemoveAt(paramIndex);
                    }

                    command.CommandText = command.CommandText.Replace("," + wrongFk + "=" + wrongFKValue, "");

                }




                DeleteOneToOneFK(item);
                List<string> deleteSortedTable = new List<string>();
                IEnumerable<PropertyInfo> properties;
                Type itemType = item.GetType();
                string tableName = itemType.Name;


                properties = itemType.GetProperties();
                deleteSortedTable.Add(tableName);

                DeleteSortTable(deleteCommands, item, ref deleteSortedTable);

                deleteSortedTable.Reverse();
                deleteSortedTable = deleteSortedTable.Distinct().ToList();
                Dictionary<string, int> tableIndex = new Dictionary<string, int>();
                int currentIndex = 0;
                foreach (var table in deleteSortedTable)
                {
                    if (!tableIndex.ContainsKey(table))
                    {
                        tableIndex.Add(table, currentIndex++);
                    }
                }

                List<DbCommand> sortedCommands = deleteCommands.OrderBy(cmd =>
                {
                    string tableName = Regex.Match(cmd.CommandText, @"DELETE FROM (\w+)").Groups[1].Value;

                    // Get the index of the table name from the dictionary
                    if (tableIndex.ContainsKey(tableName))
                    {
                        return tableIndex[tableName];
                    }
                    else
                    {
                        return int.MaxValue; 
                    }
                }).ToList();


                adoNetUtility.WriteFinal(sortedCommands);
            }

        }

        #region private helper methods
        private void DeleteRecursive(object item, string fkNameParentIList, G? parentObjectId)
        {
            if (item != null)
            {
                Type itemType = item.GetType();
                string tableName = itemType.Name;
                IEnumerable<PropertyInfo> properties;
                if (_isAutoIncremented)
                {
                    properties = itemType.GetProperties().Where(p => p.Name != "Id");
                }
                else
                {
                    properties = itemType.GetProperties();
                }

                // Generate the SQL query
                (string sql, List<DbParameter> parameters) UpdateSqlAndParameters = DeleteGenerateSqlQuery(tableName, properties,
                                                                               item, fkNameParentIList, parentObjectId);

                using (var adoNetUtility = new AdoNetUtility(_connectionString))
                {


                    foreach (PropertyInfo property in properties)
                    {
                        if (property.PropertyType.IsGenericType &&
                            property.PropertyType.GetGenericTypeDefinition() == typeof(List<>) ||
                            property.PropertyType.IsArray)
                        {


                        }
                        else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                        {

                        }
                        else
                        {
                            string propertyName = property.Name;
                            var parameter = new SqlParameter($"@{propertyName}", property.GetValue(item) ?? DBNull.Value);
                            UpdateSqlAndParameters.parameters.Add(parameter);
                        }
                    }

                    adoNetUtility.WriteOperation(UpdateSqlAndParameters.sql, UpdateSqlAndParameters.parameters, deleteCommands);

                }
            }



        }
        private void DeleteOneToOneFK(object item)
        {
            var propertyInfo = item.GetType().GetProperties();
            foreach (PropertyInfo property in propertyInfo)
            {
                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(List<>) ||
                    property.PropertyType.IsArray)
                {
                    var list = property.GetValue(item) as IList;


                    foreach (var listItem in list)
                    {

                        DeleteOneToOneFK(listItem);

                    }
                }
                else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {

                    var propertyValue = property.GetValue(item);
                    var oneToOneFK = property.Name + "Id";



                    foreach (var command in deleteCommands)
                    {

                        if (command.CommandText.Contains(" " + item.GetType().Name + " "))
                        {

                            int firstClosingParenIndex = command.CommandText.IndexOf(')');
                            command.CommandText = command.CommandText.Insert(firstClosingParenIndex, "," + oneToOneFK);

                            int lastClosingParenIndex = command.CommandText.LastIndexOf(')');
                            command.CommandText = command.CommandText.Insert(lastClosingParenIndex, ",@" + oneToOneFK);

                            var parameter = command.CreateParameter();
                            parameter.ParameterName = "@" + oneToOneFK;

                            if (propertyValue != null)
                            {
                                var idProperty = propertyValue.GetType().GetProperty("Id");
                                if (idProperty != null)
                                {
                                    parameter.Value = idProperty.GetValue(propertyValue);
                                }
                                else
                                {
                                }
                            }
                            else
                            {
                            }
                            command.Parameters.Add(parameter);

                        }
                    }
                    DeleteOneToOneFK(propertyValue);
                }
                else
                {


                }

            }


        }
        private (string, List<DbParameter>) DeleteGenerateSqlQuery(string tableName, IEnumerable<PropertyInfo> properties,
            object item, string fkNameParentIList, G? parentObjectId, string sqlColumns = null, string sqlValues = null)
        {


            List<DbParameter> parameters = new List<DbParameter>();
            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(List<>) ||
                    property.PropertyType.IsArray)
                {
                    var list = property.GetValue(item) as IList;


                    fkNameParentIList = item.GetType().Name + "Id";
                    parentObjectId = (dynamic)item.GetType().GetProperty("Id").GetValue(item);

                    foreach (var listItem in list)
                    {

                        DeleteRecursive(listItem, fkNameParentIList, parentObjectId);

                    }
                }
                else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    var propertyValue = property.GetValue(item);
                    DeleteRecursive(propertyValue, fkNameParentIList, parentObjectId);


                }
                else
                {
                    string propertyName = property.Name;
                    sqlColumns += $"{propertyName},";
                    sqlValues += $"@{propertyName},";

                    if (fkNameParentIList != null)
                    {
                        if (!sqlColumns.Contains(fkNameParentIList))
                        {
                            sqlColumns += $"{fkNameParentIList},";
                            sqlValues += $"@{fkNameParentIList},";
                            parameters.Add(new SqlParameter($"@{fkNameParentIList}", parentObjectId));
                        }

                    }

                }

            }


            sqlColumns = sqlColumns.TrimEnd(',');
            sqlValues = sqlValues.TrimEnd(',');


            // Generate the SQL query
            string sql = $"DELETE FROM {tableName}";


            return (sql, parameters);

        }


        #endregion


        public void DeleteSortTable(List<DbCommand> deleteCommands, object item, ref List<string> deleteSortedTable, bool flag = true)
        {
            IEnumerable<PropertyInfo> properties;
            Type itemType = item.GetType();
            string tableName = itemType.Name;

            properties = itemType.GetProperties().Reverse();



            foreach (PropertyInfo property in properties)
            {

                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(List<>) ||
                    property.PropertyType.IsArray)
                {
                    var list = property.GetValue(item) as IList;
                    int index = deleteSortedTable.IndexOf(item.GetType().Name);

                    foreach (var listItem in list)
                    {
                        deleteSortedTable.Insert(index + 1, listItem.GetType().Name);
                        DeleteSortTable(deleteCommands, listItem, ref deleteSortedTable, false);

                    }
                }
                else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {

                    int index = deleteSortedTable.IndexOf(item.GetType().Name);
                    deleteSortedTable.Insert(index, property.PropertyType.Name);
                    DeleteSortTable(deleteCommands, property.GetValue(item), ref deleteSortedTable, false);
                }
                else
                {

                }
            }

        }


        #endregion




        #region Delete(G id)
        public void Delete(G id)                                           // NOT Fully COMPLETE 
        {
            List<DbCommand> DeleteGIdCommand = new List<DbCommand>();

            string tableName = typeof(T).Name;
            string sql = $"DELETE FROM {tableName} WHERE Id = @Id";

            using (var adoNetUtility = new AdoNetUtility(_connectionString))
            {
                var parameters = new List<DbParameter>
                        {
                               new SqlParameter("@Id", id)
                        };
                adoNetUtility.WriteOperation(sql, parameters, DeleteGIdCommand);
                //adoNetUtility.WriteFinal(DeleteGIdCommand);
            }
        }

        #endregion




        #region GetAll()
        public List<T> GetAll()                                            // NOT Fully COMPLETE 
        {
            string tableName = typeof(T).Name;
            string sql = $"SELECT * FROM {tableName}";

            using (var adoNetUtility = new AdoNetUtility(_connectionString))
            {
                var rows = adoNetUtility.ReadOperation(sql, null, false);

                List<T> items = new List<T>();

                foreach (var row in rows)
                {
                    T item = Activator.CreateInstance<T>();

                    foreach (var prop in typeof(T).GetProperties())
                    {
                        if (prop.Name != "Id" && row.ContainsKey(prop.Name))
                        {
                            object value = row[prop.Name];
                            if (value != DBNull.Value)
                            {
                                prop.SetValue(item, value);
                            }
                        }
                    }

                    items.Add(item);
                }

                return items;
            }
        }
        #endregion




        #region GetById(G id) 
        public T GetById(G id)                                  // NOT Fully COMPLETE 
        {
            string tableName = typeof(T).Name;
            string sql = $"SELECT * FROM {tableName} WHERE Id = @Id";

            using (var adoNetUtility = new AdoNetUtility(_connectionString))
            {
                var parameter = new SqlParameter("@Id", id);
                var parameters = new List<DbParameter> { parameter };

                var rows = adoNetUtility.ReadOperation(sql, parameters, false);

                if (rows.Count == 0)
                {
                    return default(T);
                }

                var row = rows.First();

                var obj = Activator.CreateInstance<T>();

                foreach (var property in typeof(T).GetProperties())
                {
                    if (property.Name != "Id")
                    {
                        if (property.PropertyType.IsGenericType &&
                           property.PropertyType.GetGenericTypeDefinition() == typeof(List<>) ||
                           property.PropertyType.IsArray)
                        {

                        }
                        else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                        {

                        }
                        else
                        {
                            var value = row[property.Name];
                            property.SetValue(obj, value is DBNull ? null : value);
                        }
                    }
                    else
                    {
                        property.SetValue(obj, id);
                    }
                }
                return obj;
            }
        }

        #endregion


    }
}