
using static System.Net.Mime.MediaTypeNames;

namespace RGR_BD
{
    public class View
    {
        public string ShowTable(List<List<string>> rows, List<string> columnsname)
        {
            while (true) {
                string page = " ";
                Console.Clear();
                ShowCurrentTable();
                for (int i = 0; i < columnsname.Count; i++)
                {
                    Console.Write($"  {columnsname[i]}   ");
                }
                Console.WriteLine();
                for (int i = 0; i < rows.Count; i++)
                {
                    for (int j = 0; j < rows[i].Count; j++)
                    {
                        rows[i][j] = rows[i][j].Replace("\n", "").Replace("\r", "").Replace("\t", "");
                        Console.Write($"   {rows[i][j]}   ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine($"\n\nВведіть \"p\"(Для показу попередньої сторінки таблиці), \"n\"(Для показу наступної сторінки таблиці), \"q\"(Повернення в меню)");
                string input_choice = Console.ReadLine();
                if (input_choice.Equals("p") || input_choice.Equals("n") || input_choice.Equals("q"))
                {
                    Console.WriteLine($"Ви обрали : {input_choice}");
                    Thread.Sleep(1000);
                    page = input_choice;
                    return page;
                }
                else
                {
                    Console.WriteLine($"Будь ласка, оберіть правильну опцію");
                    Thread.Sleep(1000);
                }
            }
        }
        private void ShowCurrentTable()
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            string text_cur_table = $"Current table:{Controller.GetCurTable()}";
            Console.SetCursorPosition(width - text_cur_table.Length, 0);
            Console.WriteLine(text_cur_table);
            Console.SetCursorPosition(0, 0);
        }
        public List<(string Column , string Value)> GetValuesForTable(List<string> columnsname)
        {
            List<(string Column, string Value)> setColumns = new List<(string Column, string Value)>();
            Console.WriteLine("Якщо хочете залишите старе значення, залишти строку пустою для відповідної колонки");
            foreach(var column in columnsname)
            {
                Console.WriteLine(column+":");
                string value = Console.ReadLine();
                if (!string.IsNullOrEmpty(value))
                {
                    setColumns.Add((Column: $"{column}", Value: value));
                }
            }
            return setColumns;
        }
        public (int choice, string page )ShowRowsToAction(List<List<string>> rows, List<string> columnsname)
        {
            while (true)
            {
                string page = " ";
                int choice = 0;
                Console.Clear();
                ShowCurrentTable();
                for (int i = 0; i < columnsname.Count; i++)
                {
                    Console.Write($"  {columnsname[i]}   ");
                }
                Console.WriteLine();
                for (int i = 0; i < rows.Count; i++) {
                    for (int j = 0; j < rows[i].Count; j++)
                    {
                        rows[i][j] = rows[i][j].Replace("\n", "").Replace("\r", "").Replace("\t", "");
                        Console.Write($"   {rows[i][j]}   ");
                    }
                    Console.WriteLine();  
                }
                Console.WriteLine($"\n\nОберіть PK або введіть \"p\"(Для показу попередньої сторінки таблиці) або \"n\"(Для показу наступної сторінки таблиці:)");
                string input_choice = Console.ReadLine();
                if (int.TryParse(input_choice, out choice))
                {
                        return (choice, page);
                }
                else if (input_choice.Equals("p") || input_choice.Equals("n"))
                {
                    Console.WriteLine($"Ви обрали : {input_choice}");
                    Thread.Sleep(1000);
                    page = input_choice;
                    return (choice, page);
                }
                Console.WriteLine($"Будь ласка, оберіть правильну опцію");
                Thread.Sleep(1000);
            }
        }
        public int ShowTablesToChange(List<string> tables)
        {
            while (true)
            {
                int choice = 0;
                Console.Clear();
                ShowCurrentTable();
                for (int i = 0; i < tables.Count; i++)
                {
                    Console.WriteLine($"{i + 1}.{tables[i]}");
                }
                string input_choice = Console.ReadLine();
                if (int.TryParse(input_choice, out choice))
                {
                    if (choice > 0 && choice <= tables.Count)
                    {
                        Console.WriteLine($"Ви обрали таблицю: {choice}");
                        Thread.Sleep(1000);
                        return choice;
                    }
                    else
                    {
                        Console.WriteLine($"Будь ласка, введіть число від 1 до {tables.Count}");
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    Console.WriteLine($"Будь ласка, введіть число від 1 до {tables.Count}");
                    Thread.Sleep(1000);
                }
            }
        }
        public int ShowMenu()
        {
            while (true)
            {
                int choice = 0;
                Console.Clear();
                ShowCurrentTable();
                Console.WriteLine("1.Змінити поточну таблицю");
                Console.WriteLine("2.Додавання данних в поточну таблицю");
                Console.WriteLine("3.Оновлення данних в поточній таблиці");
                Console.WriteLine("4.Видалення данних з поточної таблиці");
                Console.WriteLine("5.Вивести поточну таблицю");
                Console.WriteLine("6.Вихід");
                Console.WriteLine("\nОберіть опцію:");
                string input_choice = Console.ReadLine();

                if (int.TryParse(input_choice, out choice))
                {
                    if (choice >= 1 && choice <= 5)
                    {
                        Console.WriteLine($"Ви обрали опцію: {choice}");
                        Thread.Sleep(1000);
                        return choice;
                    }
                    else
                    {
                        Console.WriteLine("Будь ласка, введіть число від 1 до 5");
                        Thread.Sleep(1500);
                    }
                }
                {
                    Console.WriteLine($"Будь ласка, введіть число від 1 до 5");
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
