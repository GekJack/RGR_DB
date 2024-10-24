
using static System.Net.Mime.MediaTypeNames;

namespace RGR_BD
{
    public class View
    {
        
        private void ShowCurrentTable()
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            string text_cur_table = $"Current table:{Controller.GetCurTable()}";
            Console.SetCursorPosition(width - text_cur_table.Length, height - 5);
            Console.WriteLine(text_cur_table);
            Console.SetCursorPosition(0, 0);
        }
        public int ShowRowsToUpdate(List<List<string>> rows, List<string> columnsname)
        {
            while (true)
            {
                int choice = 0;
                Console.Clear();
                ShowCurrentTable();
                for (int i = 0; i < columnsname.Count; i++)
                {
                    Console.Write($"  {columnsname[i]}   ");
                }
                Console.WriteLine();
                for (int i = 0; i < rows.Count; i++) {
                    Console.Write($"{i + 1}.\t");
                    for (int j = 0; j < rows[i].Count; j++)
                    {
                        rows[i][j] = rows[i][j].Replace("\n", "").Replace("\r", "").Replace("\t", "");
                        Console.Write($"{rows[i][j]}   ");
                    }
                    Console.WriteLine();  
                }
                string input_choice = Console.ReadLine();
                if (int.TryParse(input_choice, out choice))
                {
                    if (choice > 0 && choice <= rows.Count)
                    {
                        Console.WriteLine($"Ви обрали строку: {choice}");
                        Thread.Sleep(1000);
                        return choice;
                    }
                    else
                    {
                        Console.WriteLine($"Будь ласка, введіть число від 1 до {rows.Count}");
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    Console.WriteLine($"Будь ласка, введіть число від 1 до {rows.Count}");
                    Thread.Sleep(1000);
                }
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
                Console.WriteLine("5.Вихід");
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
