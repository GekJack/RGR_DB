
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
                        cycle_run = !cycle_run;
                        break;
                }
            }
        }
        private static void ChangeCurTable()
        {
            var tables_res = model.GetAllTables();
            if( tables_res.error) {
                Console.WriteLine("Continue y/n?");
                string continue_choice = Console.ReadLine();
                if(continue_choice.ToLower() == "y" || continue_choice.ToLower() == "yes") return;
                else if (continue_choice.ToLower() == "n" || continue_choice.ToLower() == "no") Environment.Exit(0);
            }
            int choice = view.ShowTablesToChange(tables_res.tables);
            current_table = tables_res.tables[choice - 1];
        }
        private static void UpdateTableData()
        {
            var rows_res = model.GetRowsOfTable(current_table);
            if (rows_res.error)
            {
                Console.WriteLine("Continue y/n?");
                string continue_choice = Console.ReadLine();
                if (continue_choice.ToLower() == "y" || continue_choice.ToLower() == "yes") return;
                else if (continue_choice.ToLower() == "n" || continue_choice.ToLower() == "no") Environment.Exit(0);
            }
            var columnsname_res = model.GetColumnNameOfTable(current_table);
            if (columnsname_res.error)
            {
                Console.WriteLine("Continue y/n?");
                string continue_choice = Console.ReadLine();
                if (continue_choice.ToLower() == "y" || continue_choice.ToLower() == "yes") return;
                else if (continue_choice.ToLower() == "n" || continue_choice.ToLower() == "no") Environment.Exit(0);
            }
            int choice = view.ShowRowsToUpdate(rows_res.rows, columnsname_res.ColumnsName);
        }
    }
}
