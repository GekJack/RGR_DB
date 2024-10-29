
using Npgsql;
using System.Data;

namespace RGR_BD
{
    public class Model
    {
        private NpgsqlConnection connection;
        public Model()
        {
            string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=Miha2004.;Database=Lab_1";
            connection = new NpgsqlConnection(connectionString);
        }
        public void CloseConnection()
        {
            try
            {   if(connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при закритті з'єднання з базою данних");
            }
        }
        public bool AddDataToTableModel(List<(string Column, string Value)> values,string table_name)
        {
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при підключенні до бази данних");
                return true;
            }
            Dictionary<string, string> columnTypes = GetColumnTypes(table_name);
            try
            {
                List<string> setClauses = new List<string>();
                List<string> setValues = new List<string>();
                foreach (var column in values)
                {
                    setClauses.Add($"{column.Column}");
                    setValues.Add($"@{column.Column}");
                }
                string setClause_str = string.Join(",", setClauses);
                string setValues_str = string.Join(",", setValues);
                string query = $"INSERT INTO {table_name} ({setClause_str}) VALUES ({setValues_str});";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    foreach (var (Column, Value) in values)
                    {
                        cmd.Parameters.AddWithValue($"@{Column}", GetConvertedValues(columnTypes, Column, Value));
                    }
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при додаванні данних " + ex.Message);
                return true;
            }
            try
            {
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при закритті з'єднання з базою данних");
                return true;
            }

            return false;
        }
        private Dictionary<string, string> GetColumnTypes(string tableName)
        {
            var columnTypes = new Dictionary<string, string>();

            string query = @$"
            SELECT column_name, data_type 
            FROM information_schema.columns 
            WHERE table_name = '{tableName}'";

            using (var cmd = new NpgsqlCommand(query, connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string columnName = reader.GetString(0);
                        string dataType = reader.GetString(1);
                        columnTypes[columnName] = dataType;
                    }
                }
            }

            return columnTypes;
        }
        public string GetPrimaryKeyColumn(string table_name)
        {
            string pk_str = string.Empty;
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при підключенні до бази данних");
                return pk_str;
            }
            try
            {
                string query = @"
                SELECT kcu.column_name 
                FROM information_schema.table_constraints AS tc
                JOIN information_schema.key_column_usage AS kcu 
                    ON kcu.constraint_name = tc.constraint_name 
                WHERE tc.table_name = @TableName 
                  AND tc.constraint_type = 'PRIMARY KEY';";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@TableName", table_name);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                           pk_str = reader["column_name"].ToString();
                        }
                        else
                        {
                           pk_str = string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при находженні первинного ключа ");
                return pk_str;
            }
            try
            {
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при закритті з'єднання з базою данних");
                return pk_str;
            }
            return pk_str;
        }
        private object GetConvertedValues(Dictionary<string, string> columnTypes, string column, string value)
        {
            object convertedValue = null;
            if (columnTypes.TryGetValue(column, out var dataType))
            {
                switch (dataType.ToLower())
                {
                    case "integer":
                        convertedValue = Convert.ToInt32(value);
                        break;
                    case "character varying":
                    case "text":
                        convertedValue = value;
                        break;
                    case "money":
                    case "numeric":
                        convertedValue = Convert.ToDecimal(value);
                        break;
                    case "boolean":
                        convertedValue = Convert.ToBoolean(value);
                        break;
                    case "date":
                        convertedValue = Convert.ToDateTime(value);
                        break;
                    case "timestamp with time zone":
                        if (DateTimeOffset.TryParse(value, out var parsedValue))
                        {
                            convertedValue = parsedValue.UtcDateTime;
                        }
                        else
                        {
                            throw new FormatException($"Невірний формат дати: {value}");
                        }
                        break;
                    default:
                        throw new ArgumentException($"Невідомий тип данних: {dataType}");
                }
            }
            return convertedValue;
        }
        public bool UpdateDataInTable(List<(string Column, string Value)> values_res, string table_name, int pk)
        {
            string pk_str_column = GetPrimaryKeyColumn(table_name);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при підключенні до бази данних");
                return true;
            }
            Dictionary<string, string> columnTypes = GetColumnTypes(table_name);
            try
            {
                List<string> setClauses = new List<string>();
                foreach (var column in values_res) {
                    setClauses.Add($"{column.Column} = @{column.Column}");
                }
                string setClause_str = string.Join(",", setClauses);
                string query = $"UPDATE {table_name} SET {setClause_str} WHERE {pk_str_column} = {pk};";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    foreach (var (Column, Value) in values_res)
                    {
                        cmd.Parameters.AddWithValue($"@{Column}", GetConvertedValues(columnTypes, Column, Value));
                    }
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при зміненні данних " + ex.Message);
                return true;
            }
            try
            {
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при закритті з'єднання з базою данних");
                return true;
            }

            return false;
        }
        public (bool error, List<string> ColumnsName) GetColumnNameOfTable(string table_name)
        {
            List<string> columnsname = new List<string>();
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при підключенні до бази данних");
                return (true, columnsname);
            }
            try
            {
                string query = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{table_name}' ORDER BY ORDINAL_POSITION;";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string columnName = reader.GetString(0);
                            columnsname.Add(columnName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при отриманні списку колонок");
                return (true, columnsname);
            }
            try
            {
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при закритті з'єднання з базою данних");
                return (true, columnsname);
            }
            return (false, columnsname);
        }
        public (bool error, List<List<string>> rows) GetRowsOfTable(string table_name, int page_num)
        {
            List<List<string>> rows = new List<List<string>>();
            int pageSize = 2;
            int startRow = (page_num - 1) * pageSize;
            string pk_str_column = GetPrimaryKeyColumn(table_name);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при підключенні до бази данних");
                return (true, rows);
            }
            try
            {
                string query = $"SELECT * FROM {table_name} ORDER BY {pk_str_column} LIMIT {pageSize} OFFSET {startRow};";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           List<string> row = new List<string>();
                           for(int i = 0; i < reader.FieldCount; i++)
                           {
                                row.Add(reader.GetValue(i).ToString());
                           }
                           rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при отриманні списку строк" + ex.Message);
                return (true, rows);
            }
            try
            {
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при закритті з'єднання з базою данних");
                return (true, rows);
            }
            return (false, rows);
        }
        public (bool error, List<string> tables) GetAllTables()
        {
            List<string> tables = new List<string>();
            try
            {
                connection.Open();
            }
            catch (Exception ex) { 
                Console.WriteLine("Помилка при підключенні до бази данних");
                return (true, tables);
            }
            try
            {
                string query = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public';";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Помилка при отриманні списку таблиць");
                return (true, tables);
            }
            try
            {
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при закритті з'єднання з базою данних");
                return (true, tables);
            }
            return (false, tables);
        }
        public bool DeleteDataOfTable(string table_name, int pk, string pk_str)
        {
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при підключенні до бази данних");
                return (true);
            }
            try
            {
                string query = $"DELETE FROM {table_name} WHERE {pk_str} = {pk}";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при видаленні рядка");
                return (true);
            }

            try
            {
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при закритті з'єднання з базою данних");
                return (true);
            }
            return false;
        }
    }
}
