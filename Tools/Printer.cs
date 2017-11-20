using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomClientLauncher.Tools
{
    class Printer
    {

        private static int INFO_LENGTH = 30;

        public static void info(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            int spaces = Printer.INFO_LENGTH - message.Length;
            for (int i = 1; i <= spaces; i++)
            {
                message += " ";
            }
            Console.ForegroundColor = color;
            Console.Write("\n\r - " + message);
        }

        public static void patchesInfo(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            int spaces = (Printer.INFO_LENGTH - 4) - message.Length;
            for (int i = 1; i <= spaces; i++)
            {
                message += " ";
            }
            Console.ForegroundColor = color;
            Console.Write("\n\r     - " + message);
        }

        public static void result(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
        }

        public static void resultOk(string message = "ok")
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(message);
        }

        public static void resultNotice(string message = "need changes")
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(message);
        }

        public static void resultError(string message = "unknown Error")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(message);
        }

    }
}
