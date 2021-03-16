using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Latis_R.Engine_Key_Constructor
{
    class Program
    {
        static int exit = 1;
        static string path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "").Replace("Latis R.Engine Key Constructor.dll", "").Replace("Latis_R.Engine_Key_Constructor.dll", "");



        static Process myProcess = Process.GetCurrentProcess();

        static string authkey;
        private static char[] inputStr;

        static void Sleep(int ms)
        {
            Thread.Sleep(ms);
        }











        static void Main(string[] args)
        {


            Console.Title = "Latis R.Engine Key Constructor: ADMIN";
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
            Console.WriteLine("Please enter auth key: \n");
            Console.ForegroundColor = ConsoleColor.Green;
            authkey = Console.ReadLine();

            string mergedPass = string.Concat(authkey.Length, authkey);
            authkey = EncryptUsingMD5(mergedPass);
            File.WriteAllText(path + "authkey.txt", authkey);
            Console.Clear();
            Console.WriteLine("Created at " + path + "authkey.txt");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press enter to continue..");
            string temp = Console.ReadLine();
        }



        private static string EncryptUsingMD5(string mergedPass)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(mergedPass));

                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                    sBuilder.Append(data[i].ToString("x2"));

                return sBuilder.ToString();
            }
        }
    }
}
