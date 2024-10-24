using System;
using System.Text;
using Npgsql;
namespace RGR_BD
{
    class MainClass
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(200, 30);
            Console.SetBufferSize(200, 30);
            Console.OutputEncoding = Encoding.UTF8;
            Controller.Run();
        }
    }
}
