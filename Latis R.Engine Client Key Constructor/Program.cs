using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Latis_R.Engine_Key_Constructor
{
    class Program
    {
        static int exit = 1;
        static string path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "").Replace("Latis R.Engine Key Constructor.exe", "").Replace("Latis_R.Engine_Key_Constructor.exe", "");

        static Process myProcess = Process.GetCurrentProcess();



        static void Sleep(int ms)
        {
            Thread.Sleep(ms);
        }





        static void Main(string[] args)
        {
            Console.Title = "Latis R.Engine Key Constructor: CLIENT";
            myProcess.PriorityClass = ProcessPriorityClass.RealTime;

            Banner();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Welcome to the Key Constructor!\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press enter to continue..");
            string temp = Console.ReadLine();
            while (exit == 1)
            {
                MainMenu();
            }
        }





        static void Banner()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(" __      __   ____  ____  ___    ___  _____  ____  ____  _    _    __    ____  ____ \n(  )    /__\\ (_  _)(_  _)/ __)  / __)(  _  )( ___)(_  _)( \\/\\/ )  /__\\  (  _ \\( ___)\n )(__  /(__)\\  )(   _)(_ \\__ \\  \\__ \\ )(_)(  )__)   )(   )    (  /(__)\\  )   / )__) \n(____)(__)(__)(__) (____)(___/  (___/(_____)(__)   (__) (__/\\__)(__)(__)(_)\\_)(____)");
            Console.ForegroundColor = ConsoleColor.White;
            Sleep(3000);
            Console.Clear();
        }






        static void MainMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Please enter username: \n");
            Console.ForegroundColor = ConsoleColor.White;
            string username = Console.ReadLine();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Please enter password: \n");
            Console.ForegroundColor = ConsoleColor.White;
            string password = Console.ReadLine();
            Console.Clear();

            string authkey = PersoCrypt(username, password);
            File.WriteAllText(path + "authkey.txt", authkey);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Created at " + path + "authkey.txt");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press enter to continue..");
            string temp = Console.ReadLine();
        }

        static string PersoCrypt(string user, string password)
        {

            string guid = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Cryptography", "MachineGuid", "none").ToString();

            guid = guid.Replace("-", "");
            guid = guid.Replace("a", "z");
            guid = guid.Replace("b", "v");
            guid = guid.Replace("c", "f");
            guid = guid.Replace("d", "0");
            guid = guid.Replace("e", "y");
            guid = guid.Replace("f", "a");
            guid = guid.Replace("g", "9");
            guid = guid.Replace("h", "d");
            guid = guid.Replace("i", "c");
            guid = guid.Replace("j", "8");
            guid = guid.Replace("k", "i");
            guid = guid.Replace("l", "k");
            guid = guid.Replace("m", "n");
            guid = guid.Replace("n", "a");
            guid = guid.Replace("o", "k");
            guid = guid.Replace("p", "e");
            guid = guid.Replace("q", "1");
            guid = guid.Replace("r", "v");
            guid = guid.Replace("s", "a");
            guid = guid.Replace("t", "m");
            guid = guid.Replace("u", "1");
            guid = guid.Replace("v", "v");
            guid = guid.Replace("w", "j");
            guid = guid.Replace("x", "4");
            guid = guid.Replace("y", "c");
            guid = guid.Replace("0", "v");
            guid = guid.Replace("1", "8");
            guid = guid.Replace("2", "a");
            guid = guid.Replace("3", "a");
            guid = guid.Replace("4", "2");
            guid = guid.Replace("5", "7");
            guid = guid.Replace("6", "g");
            guid = guid.Replace("7", "r");
            guid = guid.Replace("8", "1");
            guid = guid.Replace("9", "0");

            user = user.Replace("a", "z");
            user = user.Replace("b", "E");
            user = user.Replace("c", "f");
            user = user.Replace("d", "0");
            user = user.Replace("e", "y");
            user = user.Replace("f", "a");
            user = user.Replace("g", "9");
            user = user.Replace("h", "d");
            user = user.Replace("i", "c");
            user = user.Replace("j", "8");
            user = user.Replace("k", "Be");
            user = user.Replace("l", "D");
            user = user.Replace("m", "n");
            user = user.Replace("n", "v");
            user = user.Replace("o", "k");
            user = user.Replace("p", "b");
            user = user.Replace("q", "1");
            user = user.Replace("r", "v");
            user = user.Replace("s", "8");
            user = user.Replace("t", "T");
            user = user.Replace("u", "1");
            user = user.Replace("v", "9");
            user = user.Replace("w", "j");
            user = user.Replace("x", "4");
            user = user.Replace("y", "f");
            user = user.Replace("0", "v");
            user = user.Replace("1", "(");
            user = user.Replace("2", "a");
            user = user.Replace("3", "a");
            user = user.Replace("4", "2");
            user = user.Replace("5", "Ve");
            user = user.Replace("6", "g");
            user = user.Replace("7", "r");
            user = user.Replace("8", "'");
            user = user.Replace("9", "0~");

            password = password.Replace("a", "z");
            password = password.Replace("b", "E");
            password = password.Replace("c", "m");
            password = password.Replace("d", "0");
            password = password.Replace("e", "v");
            password = password.Replace("f", "a");
            password = password.Replace("g", "é");
            password = password.Replace("h", "d");
            password = password.Replace("i", "|.");
            password = password.Replace("j", "8");
            password = password.Replace("k", "Be");
            password = password.Replace("l", "D");
            password = password.Replace("m", "n");
            password = password.Replace("n", "8-");
            password = password.Replace("o", "k");
            password = password.Replace("p", "b");
            password = password.Replace("q", "1");
            password = password.Replace("r", "n");
            password = password.Replace("s", "è");
            password = password.Replace("t", "T");
            password = password.Replace("u", "GVR");
            password = password.Replace("v", "9");
            password = password.Replace("w", "j");
            password = password.Replace("x", "E");
            password = password.Replace("y", "f");
            password = password.Replace("0", "v");
            password = password.Replace("1", "v");
            password = password.Replace("2", "a/");
            password = password.Replace("3", "a");
            password = password.Replace("4", "2");
            password = password.Replace("5", "Ve");
            password = password.Replace("6", "g");
            password = password.Replace("7", "r");
            password = password.Replace("8", "'");
            password = password.Replace("9", "0~");

            string authkey = user + password + guid;
            return authkey;
        }
    }
}
