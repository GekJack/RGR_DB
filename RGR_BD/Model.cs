
using Npgsql;

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
        public (bool error, List<List<string>> rows) GetRowsOfTable(string table_name)
        {
            List<List<string>> rows = new List<List<string>>();
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
                string query = $"SELECT * FROM {table_name};";
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
                Console.WriteLine("Помилка при отриманні списку строк");
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
    }
}
