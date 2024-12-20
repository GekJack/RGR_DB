﻿namespace RGR_BD
{
    public class Controller
    {
        static private Model model = new Model();
        static private View view = new View();
        private static string current_table = "None";
        public static string GetCurTable()
        {
            return current_table;
        }
        public static void Run()
        {
            bool cycle_run = true;
            while (cycle_run)
            {
                int choice = view.ShowMenu();
                if (current_table == "None" && choice < 7 && choice != 1)
                {
                    Console.WriteLine("Оберіть таблицю для роботи!!!");
                    Thread.Sleep(1000);
                }
                else
                {
                    switch (choice)
                    {
                        case 1:
                            ChangeCurTable();
                            break;
                        case 2:
                            AddDataToTable();
                            break;
                        case 3:
                            UpdateTableData();
                            break;
                        case 4:
                            DeleteData();
                            break;
                        case 5:
                            ShowCurTable();
                            break;
                        case 6:
                            GenerateData();
                            break;
                        case 7:
                            Search(1);
                            break;
                        case 8:
                            Search(2);
                            break;
                        case 9:
                            Search(3);
                            break;
                        case 10:
                            cycle_run = !cycle_run;
                            break;
                    }
                }
                
            }
        }
        private static void Search(int num_of_search)
        {
            if (num_of_search == 1)
            {
                string str_choice = view.GetParamForSearch(1).ToString();
                var res = model.SearchFirst(str_choice);
                if (ContinueText(res.error))
                {
                    return;
                }
                view.ShowResOfSearch(res.str_res, num_of_search, res.time);
            }
            if (num_of_search == 2)
            {
                string str_choice = view.GetParamForSearch(2).ToString();
                var res = model.SearchSecond(str_choice);
                if (ContinueText(res.error))
                {
                    return;
                }
                view.ShowResOfSearch(res.str_res, num_of_search, res.time);
            }
            if (num_of_search == 3)
            {
                var str_choice = view.GetParamForSearch(3);
                if (str_choice is (string city, string expYears))
                {
                    var res = model.SearchThird(city, expYears);
                    if (ContinueText(res.error))
                    {
                        return;
                    }
                    view.ShowResOfSearch(res.str_res, num_of_search, res.time);
                }
            }
        }
        private static void DeleteData()
        {
            var columnsname_res = model.GetColumnNameOfTable(current_table);
            if (ContinueText(columnsname_res.error))
            {
                return;
            }
            var rows_res = model.GetRowsOfTable(current_table, 1);
            if (ContinueText(rows_res.error))
            {
                return;
            }
            int i = 1;
            (int choice, string page) res;
            while (true)
            {
                res = view.ShowRowsToAction(rows_res.rows, columnsname_res.ColumnsName);
                if (res.page.Equals("n"))
                {
                    i++;
                }
                else if (res.page.Equals("p") && i > 1)
                {
                    i--;
                }
                else if (!res.page.Equals(" "))
                {
                    Console.WriteLine("Ви на першій сторінці");
                    Thread.Sleep(1000);
                }
                if (res.page.Equals(" "))
                {
                    break;
                }
                rows_res = model.GetRowsOfTable(current_table, i);
                if (ContinueText(rows_res.error))
                {
                    return;
                }
            }
            string pk_str = model.GetPrimaryKeyColumn(current_table);
            bool error = model.DeleteDataOfTable(current_table, res.choice, pk_str);
            if (ContinueText(error)) return;
        }
        private static bool ContinueText(bool error)
        {
            if (error)
            {
                Console.WriteLine("Continue y/n?");
                string continue_choice = Console.ReadLine();
                if (continue_choice.ToLower() == "y" || continue_choice.ToLower() == "yes") return true;
                else if (continue_choice.ToLower() == "n" || continue_choice.ToLower() == "no") Environment.Exit(0);
            }
            return false ;
        }
        private static void AddDataToTable()
        {
            var columnsname = model.GetColumnNameOfTable(current_table);
            if (ContinueText(columnsname.error))
            {
                return;
            }
            string pk_str_name = model.GetPrimaryKeyColumn(current_table);
            columnsname.ColumnsName.RemoveAll(item => item.Equals(pk_str_name));
            var data_to_add = view.GetValuesForTable(columnsname.ColumnsName, false);
            bool error = model.AddDataToTableModel(data_to_add, current_table);
            if (ContinueText(error))
            {
                return;
            }
        }
        private static void ShowCurTable()
        {
            var table = model.GetRowsOfTable(current_table, 1);
            if (ContinueText(table.error))
            {
               return ;
            }
            var table_columns = model.GetColumnNameOfTable(current_table);
            if (ContinueText(table_columns.error))
            {
                return;
            }
            int i = 1;
            while (true)
            {
                string res = view.ShowTable(table.rows, table_columns.ColumnsName);
                if (res.Equals("n"))
                {
                    i++;
                }else if (res.Equals("p") && i > 1)
                {
                    i--;
                }
                else if (res.Equals("q"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ви на першій сторінці");
                    Thread.Sleep(1000);
                }
                table = model.GetRowsOfTable(current_table, i);
                if (ContinueText(table.error))
                {
                    return;
                }
            }

        }
        private static void ChangeCurTable()
        {
            var tables_res = model.GetAllTables();
            if (ContinueText(tables_res.error))
            {
                return;
            }
            int choice = view.ShowTablesToChange(tables_res.tables);
            current_table = tables_res.tables[choice - 1];
        }
        private static void UpdateTableData()
        {
            var columnsname_res = model.GetColumnNameOfTable(current_table);
            if (ContinueText(columnsname_res.error))
            {
                return;
            }
            var rows_res = model.GetRowsOfTable(current_table, 1);
            if (ContinueText(rows_res.error))
            {
                return;
            }
            int i = 1;
            (int choice, string page) res;
            while (true)
            {
                res = view.ShowRowsToAction(rows_res.rows, columnsname_res.ColumnsName);
                if(res.page.Equals("n"))
                {
                    i++;
                }else if(res.page.Equals("p") && i > 1)
                {
                    i--;
                }
                else if(!res.page.Equals(" "))
                {
                    Console.WriteLine("Ви на першій сторінці");
                    Thread.Sleep(1000);
                } 
                if(res.page.Equals(" "))
                {
                    break;
                }
                rows_res = model.GetRowsOfTable(current_table, i);
                if (ContinueText(rows_res.error))
                {
                    return;
                }
            }
            string pk_str_name = model.GetPrimaryKeyColumn(current_table);
            columnsname_res.ColumnsName.RemoveAll(item => item.Equals(pk_str_name));
            List<(string Column, string Value)> values_res = view.GetValuesForTable(columnsname_res.ColumnsName, true);
            bool error = model.UpdateDataInTable(values_res, current_table,res.choice);
            if (ContinueText(error))
            {
               return;
            }

        }
        private static void GenerateData()
        {
            int choice = view.GetCountRowsToGenerate();
            string proc_name = $"generate_{current_table}";
            bool error = model.GenerateDataToCurrentTable(proc_name, choice);
            if (ContinueText(error))
            {
                return;
            }
            Console.WriteLine($"Було успішно згенеровано {choice} строк(Для продовження натисніть будь-яку клавішу)");
            Console.ReadLine();
        }
    }
}
