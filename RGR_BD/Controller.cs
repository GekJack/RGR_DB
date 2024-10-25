﻿
namespace RGR_BD
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
                switch (choice)
                {
                    case 1:
                        ChangeCurTable();
                        break;
                    case 2:
                        if (current_table != "None")
                        {
                            AddDataToTable();
                        }
                        else
                        {
                            Console.WriteLine("Оберіть таблицю для роботу!!!");
                        }
                        break;
                    case 3:
                        if(current_table != "None")
                        {
                            UpdateTableData();
                        }
                        else
                        {
                            Console.WriteLine("Оберіть таблицю для роботу!!!");
                        }
                        break;
                    case 4:
                        break;
                    case 5:
                        if (current_table != "None")
                        {
                            ShowCurTable();
                        }
                        else
                        {
                            Console.WriteLine("Оберіть таблицю для роботу!!!");
                        }
                        break;
                    case 6:
                        cycle_run = !cycle_run;
                        break;
                }
            }
        }
        private static bool ContinueText(bool error)
        {
            if (error)
            {
                Console.WriteLine("Continue y/n?");
                string continue_choice = Console.ReadLine();
                if (continue_choice.ToLower() == "y" || continue_choice.ToLower() == "yes") { model.CloseConnection(); return true ; }
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
            var data_to_add = view.GetValuesForTable(columnsname.ColumnsName, false);
            string pk_str_name = model.GetPrimaryKeyColumn(current_table);
            data_to_add.RemoveAll(item => item.Column.Equals(pk_str_name));
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
            List<(string Column, string Value)> values_res = view.GetValuesForTable(columnsname_res.ColumnsName, true);
            bool error = model.UpdateDataInTable(values_res, current_table,res.choice);
            if (ContinueText(error))
            {
               return;
            }

        }
    }
}
