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
        private void ShowColumnForSearch(List<string> columns)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                Console.Write($"   {columns[i]}   ");
            }
            Console.WriteLine();
        }
        public void ShowResOfSearch(List<List<string>> rows, int num_of_search, long time)
        {
            List<string> search_f = new List<string>() { "status", "total_bookings", "total_cost" };
            List<string> search_s = new List<string>() {"count_max_participants", "city", "count_total_bookings" };
            List<string> search_t = new List<string>() { "city","rating", "avg_exp_years","avg_price" };
            Console.Clear();
            while (true)
            {
                switch (num_of_search)
                {
                    case 1:
                        ShowColumnForSearch(search_f);
                        break;
                    case 2:
                        ShowColumnForSearch(search_s);
                        break;
                    case 3:
                        ShowColumnForSearch(search_t);
                        break;
                }
                for (int i = 0; i < rows.Count; i++)
                {
                    for (int j = 0; j < rows[i].Count; j++)
                    {
                        rows[i][j] = rows[i][j].Replace("\n", "").Replace("\r", "").Replace("\t", "");
                        Console.Write($"   {rows[i][j]}   ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine($"Час виконання пошуку(запроса до БД) = {time}ms");
                Console.WriteLine($"\n\nВведіть \"q\"(Повернення в меню)");
                string input_choice = Console.ReadLine();
                if (input_choice.Equals("q"))
                {
                    Console.WriteLine($"Ви обрали : {input_choice}");
                    Thread.Sleep(1000);
                    return;
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
        public List<(string Column , string Value)> GetValuesForTable(List<string> columnsname, bool flag)
        {
            List<(string Column, string Value)> setColumns = new List<(string Column, string Value)>();
            if(flag) Console.WriteLine("Якщо хочете залишите старе значення, залишти строку пустою для відповідної колонки");
            else Console.WriteLine("Задайте значення для нової строки в таблиці");
            foreach (var column in columnsname)
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
        public object GetParamForSearch(int num_of_search)
        {
            if(num_of_search == 1)
            {
                Console.WriteLine("Напишіть за яким статусом робити пошук(true або false)");
                string res_choice = Console.ReadLine();
                return res_choice;
            }
            if (num_of_search == 2)
            {
                Console.WriteLine("Напишіть від якою дати шукати(Приклад: \'2024-01-01\')");
                string res_choice = Console.ReadLine();
                return res_choice;
            }
            else
            {
                Console.WriteLine("Напишіть місто за яким буде йти пошук та мінімальну кількість років досвіду");
                Console.Write("Місто:");
                string city = Console.ReadLine();
                Console.Write("Роки досвіду:");
                string exp_years = Console.ReadLine();
                return (city, exp_years);
            }
        }
        public int GetCountRowsToGenerate()
        {
            while (true)
            {
                int choice = 0;
                Console.WriteLine($"Будь ласка, введіть кількість строк для формування");
                string input_choice = Console.ReadLine();
                if (int.TryParse(input_choice, out choice))
                {
                    return choice;
                }
                else
                {
                    Console.WriteLine($"Будь ласка, введіть число");
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
                Console.WriteLine("6.Згенерувати випадкові записи до поточної таблиці");
                Console.WriteLine("7.Знайти кількість і ціну бронювань залежно від статусу");
                Console.WriteLine("8.Знайти максимальне кількість місць та кількість бронювань за датою та містом");
                Console.WriteLine("9.Знайти середній досвід роботи тренерів та середню оплату заняття для кожного рейтингу за містом(З вказанням від якого досвіду шукати)");
                Console.WriteLine("10.Вихід");
                Console.WriteLine("\nОберіть опцію:");
                string input_choice = Console.ReadLine();

                if (int.TryParse(input_choice, out choice))
                {
                    if (choice >= 1 && choice <= 10)
                    {
                        Console.WriteLine($"Ви обрали опцію: {choice}");
                        Thread.Sleep(1000);
                        return choice;
                    }
                    else
                    {
                        Console.WriteLine("Будь ласка, введіть число від 1 до 10");
                        Thread.Sleep(1500);
                    }
                }
                {
                    Console.WriteLine($"Будь ласка, введіть число від 1 до 10");
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
