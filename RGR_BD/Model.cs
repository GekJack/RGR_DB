using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace RGR_BD
{
    public class Model
    {
        public Model()
        {
        }
        public bool AddDataToTableModel(List<(string Column, string Value)> values,string table_name)
        {       
            try
            {
                using (var context = new AppDbContext())
                {
                    var entityType = context.Model.GetEntityTypes()
                                          .FirstOrDefault(e => e.GetTableName() == table_name);
                    var entity = Activator.CreateInstance(entityType.ClrType);
                    foreach (var (column, value) in values)
                    {
                        var property = entityType.ClrType.GetProperties()
                                .FirstOrDefault(p =>
                                p.GetCustomAttribute<ColumnAttribute>()?.Name.Equals(column, StringComparison.OrdinalIgnoreCase) == true);
                        if (property != null)
                        {
                            object convertedValue = null;
                            if (property.PropertyType == typeof(DateTime))
                            {
                                convertedValue = DateTime.Parse(value).ToUniversalTime();
                            }
                            else
                            {
                                convertedValue = Convert.ChangeType(value, property.PropertyType);
                            }
                            property.SetValue(entity, convertedValue);
                            Console.WriteLine(convertedValue);
                        }
                        else
                        {
                            throw new Exception($"Столбец {column} не знайдено в {entityType.Name}");
                        }
                        var dbSetMethod = typeof(DbContext).GetMethods()
                            .First(m => m.Name == "Set" && m.IsGenericMethodDefinition)
                            .MakeGenericMethod(entityType.ClrType);

                        var dbSet = dbSetMethod.Invoke(context, null);

                        var addMethod = dbSet.GetType().GetMethod("Add");
                        addMethod.Invoke(dbSet, new[] { entity });
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при додаванні данних " + ex.Message + " " + ex.InnerException.Message);
                return true;
            }

            return false;
        }
        public string GetPrimaryKeyColumn(string table_name)
        {
            string pk_str = string.Empty;
            using (var context = new AppDbContext()){ 
                try
                {
                    var entityType = context.Model.GetEntityTypes()
                                           .FirstOrDefault(e => e.GetTableName() == table_name);
                    if (entityType == null)
                    {
                        throw new Exception($"Для таблиці {table_name} не знайдено метадані.");
                    }
                    var pk = entityType.FindPrimaryKey();
                    if (pk == null)
                    {
                        throw new Exception($"Для таблиці {table_name} не знайдено pk.");
                    }
                    pk_str = pk.Properties.FirstOrDefault()?.Name;
                }
                catch (Exception ex) {
                    Console.WriteLine("Помилка при отриманні pk " + ex.Message);
                    return pk_str;
                }
            }
            return pk_str;
        }
        public bool UpdateDataInTable(List<(string Column, string Value)> values_res, string table_name, int pk)
        {
            string pk_str_column = GetPrimaryKeyColumn(table_name);
            try
            {
                using (var context = new AppDbContext())
                {
                    var entityType = context.Model.GetEntityTypes()
                                          .FirstOrDefault(e => e.GetTableName() == table_name);
                    var genericSetMethod = typeof(DbContext)
                                   .GetMethods()
                                   .First(m => m.Name == nameof(DbContext.Set) && m.IsGenericMethodDefinition)
                                   .MakeGenericMethod(entityType.ClrType);

                    var set = (IQueryable)genericSetMethod.Invoke(context, null);
                    var castedSet = set as IQueryable;

                    var entity = castedSet.Cast<object>()
                        .FirstOrDefault(e => EF.Property<int>(e, pk_str_column) == pk);
                    if (entity != null)
                    {
                        foreach (var (column, value) in values_res)
                        {
                            var property = entityType.ClrType.GetProperties()
                                .FirstOrDefault(p =>
                                p.GetCustomAttribute<ColumnAttribute>()?.Name.Equals(column, StringComparison.OrdinalIgnoreCase) == true);
                            if (property != null)
                            {
                                object convertedValue = null;

                                if(property.PropertyType == typeof(DateTime))
                                {
                                    convertedValue = DateTime.Parse(value).ToUniversalTime();
                                }
                                else
                                {
                                    convertedValue = Convert.ChangeType(value, property.PropertyType);
                                }
                                property.SetValue(entity, convertedValue);
                            }
                            else
                            {
                                throw new Exception($"Столбець {column} не знайдено в таблиці.");
                            }
                        }
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception($"Запис з номером {pk} не знайдено.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при зміненні данних " + ex.Message);
                return true;
            }
            return false;
        }
        public (bool error, List<string> ColumnsName) GetColumnNameOfTable(string table_name)
        {
            List<string> columnsname = new List<string>();
            try
            {
                using (var context = new AppDbContext())
                {
                    var designTimeModel = context.GetService<IDesignTimeModel>();
                    var model = designTimeModel.Model;
                    var entityType = model.GetEntityTypes()
                                                 .FirstOrDefault(e => e.GetTableName() == table_name);

                    if (entityType == null)
                    {
                        throw new Exception($"Таблица {table_name} не найдена в модели.");
                    }

                    columnsname = entityType.GetProperties()
                                             .OrderBy(p => p.GetColumnOrder() ?? int.MaxValue)
                                             .Select(p => p.GetColumnName())                  
                                             .ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при отриманні назв столбців "+ex.Message);
                return (true, columnsname);
            }
            return (false, columnsname);
        }
        public (bool error, List<List<string>> rows) GetRowsOfTable(string table_name, int page_num)
        {
            List<List<string>> rows = new List<List<string>>();
            int pageSize = 50;
            int startRow = (page_num - 1) * pageSize;
            string pkPropertyName = GetPrimaryKeyColumn(table_name);
            try {
                using (var context = new AppDbContext())
                {
                    var entityClrType = context.Model.GetEntityTypes()
                                           .FirstOrDefault(e => e.GetTableName() == table_name).ClrType;
                    var genericSetMethod = typeof(DbContext)
                                   .GetMethods()
                                   .First(m => m.Name == nameof(DbContext.Set) && m.IsGenericMethodDefinition)
                                   .MakeGenericMethod(entityClrType);

                    var set = (IQueryable)genericSetMethod.Invoke(context, null);
                    var query = set
                        .OrderBy($"{pkPropertyName} ASC") 
                        .Skip(startRow)                   
                        .Take(pageSize);                  

                    foreach (var entity in query)
                    {
                        var row = new List<string>();

                        var properties = entity.GetType().GetProperties();
                        foreach (var property in properties)
                        {
                            var value = property.GetValue(entity)?.ToString() ?? string.Empty;
                            row.Add(value);
                        }

                        rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при отриманні строк {table_name} {ex.Message}");
                return (true, rows);
            }
            return (false, rows);
        }
        public (bool error, List<string> tables) GetAllTables()
        {
            List<string> tables = new List<string>();
            try
            {
                using (var context = new AppDbContext())
                {
                    var tableNames = context.Model.GetEntityTypes()
                        .Select(t => t.GetTableName()) 
                        .ToList();

                    tables.AddRange(tableNames);
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Помилка при отриманні списку таблиць " + ex.Message );
                return (true, tables);
            }
            return (false, tables);
        }
        public bool DeleteDataOfTable(string table_name, int pk, string pk_str)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var entityType = context.Model.GetEntityTypes()
                                           .FirstOrDefault(e => e.GetTableName() == table_name);
                    if (entityType == null)
                    {
                        throw new Exception($"Для таблиці {table_name} не знайдено метадані.");
                    }
                    var genericSetMethod = typeof(DbContext)
                                   .GetMethods()
                                   .First(m => m.Name == nameof(DbContext.Set) && m.IsGenericMethodDefinition)
                                   .MakeGenericMethod(entityType.ClrType);
                    var set = (IQueryable)genericSetMethod.Invoke(context, null);
                    var pkProperty = entityType.FindPrimaryKey()?.Properties.FirstOrDefault(p => p.Name == pk_str);

                    if (pkProperty == null)
                    {
                        throw new Exception($"Поле {pk_str} не является первичным ключом.");
                    }

                    var castedSet = set as IQueryable;

                    var entity = castedSet.Cast<object>()
                        .FirstOrDefault(e => EF.Property<int>(e, pk_str) == pk);

                    if (entity != null)
                    {
                        context.Remove(entity);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception($"Запис з номером {pk} не знайдено.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при видаленні данних {ex.Message}");
                return (true);
            }
            return false;
        }
        public bool GenerateDataToCurrentTable(string proc_name, int count_rows)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var query = $"CALL {proc_name}({count_rows})";
                    context.Database.ExecuteSqlRaw(query);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при генерації випадкових данних" + ex.Message);
                return true;
            }
            return false;
        }

        public (bool error, List<List<string>> str_res, long time) SearchFirst(string status)
        {
            List<List<string>> rows = new List<List<string>>();
            long executionTimeMs = 0;
                //string query = $@"SELECT 
                //                        b.status,
                //                        COUNT(b.booking_id) AS total_bookings,
                //                        SUM(s.price) AS total_cost
                //                    FROM 
                //                        bookings b
                //                    JOIN 
                //                        session s ON b.session_id = s.session_id
                //                    WHERE 
                //                        b.status = {status}
                //                    GROUP BY 
                //                        b.status
                //                    ORDER BY 
                //                        total_bookings DESC;";
                Stopwatch stopwatch = Stopwatch.StartNew();
            try {
                using (var context = new AppDbContext())
                {
                    var result = context.Bookings
                        .Where(b => b.Status == bool.Parse(status))
                        .Join(
                            context.Sessions,
                            b => b.SessionId,
                            s => s.SessionId,
                            (b, s) => new { b.Status, BookingId = b.BookingId, Price = (decimal)s.PriceMoney }
                        )
                        .GroupBy(
                            bs => bs.Status,
                            bs => new { bs.BookingId, bs.Price }
                        )
                        .Select(g => new
                        {
                            Status = g.Key,
                            TotalBookings = g.Count(),
                            TotalCost = g.Sum(x => x.Price)
                        })
                        .OrderByDescending(x => x.TotalBookings)
                        .ToList();
                    rows = result.Select(r => new List<string>
                    {
                        r.Status.ToString(),
                        r.TotalBookings.ToString(),
                        r.TotalCost.ToString()
                    }).ToList();
                    stopwatch.Stop();
                    executionTimeMs = stopwatch.ElapsedMilliseconds;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при пошукову запиті №1" + ex.Message);
                return (true, rows, 0);
            }
            return (false, rows, executionTimeMs);
        }
        public (bool error, List<List<string>> str_res, long time) SearchSecond(string start_time)
        {
            List<List<string>> rows = new List<List<string>>();
            long executionTimeMs = 0;
            try
            {
                //string query = $@"SELECT 
                //                    SUM(s.max_participants) AS count_max_participants,
                //                    l.city,
                //                 count(b.booking_id) as count_total_bookings
                //                FROM 
                //                    session s
                //                JOIN 
                //                    locations l ON s.location_id = l.location_id
                //                JOIN 
                //                    bookings b ON s.session_id = b.session_id
                //                WHERE          
                //                    s.start_time >= '{start_time}'
                //                GROUP BY
                //                    l.city
                //                ORDER BY 
                //                    count_max_participants DESC;";
                Stopwatch stopwatch = Stopwatch.StartNew();
                using (var context = new AppDbContext())
                {
                    DateTime parsedStartTime;
                    if (!DateTime.TryParse(start_time, out parsedStartTime))
                    {
                        throw new Exception("Невірний формат дати.");
                        
                    }
                    var result = context.Sessions
                        .Where(s => s.StartTime >= parsedStartTime.ToUniversalTime())
                        .Join(
                            context.Locations,
                            s => s.LocationId,
                            l => l.LocationId,
                            (s, l) => new { s, l }
                        )
                        .Join(
                            context.Bookings,
                            sl => sl.s.SessionId,
                            b => b.SessionId,
                            (sl, b) => new { sl.s, sl.l, b }
                        )
                        .GroupBy(
                            x => x.l.City,
                            x => new { x.s.MaxParticipants, x.b.BookingId }
                        )
                        .Select(g => new
                        {
                            City = g.Key,
                            CountMaxParticipants = g.Sum(x => x.MaxParticipants),
                            CountTotalBookings = g.Count()
                        })
                        .OrderByDescending(x => x.CountMaxParticipants)
                        .ToList();
                    rows = result.Select(r => new List<string>
                    {
                        r.City,
                        r.CountMaxParticipants.ToString(),
                        r.CountTotalBookings.ToString()
                    }).ToList();
                }
                stopwatch.Stop();
                executionTimeMs = stopwatch.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при пошукову запиті №2" + ex.Message);
                return (true, rows, 0);
            }
            return (false, rows, executionTimeMs);
        }
        public (bool error, List<List<string>> str_res, long time) SearchThird(string city, string exp_years)
        {
            long executionTimeMs = 0;
            List<List<string>> rows = new List<List<string>>();
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                using (var context = new AppDbContext())
                {
                    string query = $@"
                            SELECT 
                                l.city,
                                i.rating,
                                AVG(i.experiance_years) AS av_exp_yars,
                                AVG(s.price::numeric) AS avg_price
                            FROM 
                                instructors i
                            JOIN 
                                session s ON i.instructor_id = s.instructor_id
                            JOIN 
                                locations l ON s.location_id = l.location_id
                            WHERE        
                                l.city = '{city}' 
                                AND i.experiance_years >= {exp_years}
                            GROUP BY 
                                l.city, i.rating
                            ORDER BY 
                                i.rating DESC";
                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = query;
                        command.CommandType = System.Data.CommandType.Text;

                        context.Database.OpenConnection();

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var row = new List<string>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    row.Add(reader[i]?.ToString() ?? string.Empty);
                                }
                                rows.Add(row);
                            }
                        }
                    }

                    stopwatch.Stop();
                    executionTimeMs = stopwatch.ElapsedMilliseconds;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при пошукову запиті №3" + ex.Message);
                return (true, rows, 0);
            }
            return (false, rows, executionTimeMs);
        }
    }
}
