using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Latis_R.Engine
{
    class MainClass
    {
        static int exit = 1;
        static string path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "").Replace("/Latis R.Engine.exe", "/config").Replace("/Latis_R.Engine.exe", "/config");




        static Process myProcess = Process.GetCurrentProcess();




        const int moduleNum = 3;
        static bool configActive = true;
        static int config_num = 0 + moduleNum;
        static int totalModule = 0 + moduleNum;
        static bool invalidInput = false;




        static List<string> module_name = new List<string>();
        static List<string> module_file = new List<string>();
        static List<string> module_list = new List<string>();
        static List<int> module_num = new List<int>();


        static bool displayError = true;
        static bool loadModuleDisplay = true;
        static bool registryApplyDisplay = true;


        const string version = "v.1.1";
        const string build = "34";
        const string configVer = "1";

        

        static int Main(string[] args)
        {
            SetUp();
            Banner();
            Logs("Banner launch.");
            InitConfig();
            Logs("InitConfig launch.");
            while (exit == 1)
            {
                MainMenu();
            }
            return 0;
        }

        static void SetUp()
        {
            Console.Title = "Latis R.Engine " + version;
            string filecontent = null;


            DateTime now = DateTime.Now;
            File.WriteAllText(path.Replace("/config", "/logs.txt"), now + " logs\n\n=========================");
            Logs("Setup launch.");

            try
            {
                filecontent = File.ReadAllText(path.Replace("/config", "/conf.yml"));
            }
            catch (FileNotFoundException ex)
            {
                if (ex.GetType().Name.Contains("FileNotFoundException"))
                {

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("conf.yml not found creating it at: '" + path.Replace("/config", "/conf.yml") + "'..");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Please press enter to continue..");
                    string temp = Console.ReadLine();
                    Console.Clear();
                    File.WriteAllText(path.Replace("/config", "/conf.yml"), "version:" + configVer + "\nlicenseKey:\n\n#Value: true/false\ndisplayError:true\ndisplayBuild:false\nloadModuleDisplay:true\nregistryApplyDisplay:true\nrealtimePriority:true");
                    myProcess.PriorityClass = ProcessPriorityClass.RealTime;
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Put your license key in the conf.yml at: " + path.Replace("/config", "/conf.yml"));
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Please press enter to continue..");
                    temp = Console.ReadLine();
                    myProcess.Kill();
                }
            }


                if (filecontent.Contains("version:" + configVer))
            {

                if (Login() == false)
                {
                    Login();
                }


                if (filecontent.Contains("displayError:false"))
                {
                    displayError = false;
                }


                if (filecontent.Contains("displayBuild:true"))
                {
                    Console.Title = "Latis R.Engine " + version + " - BUILD: " + build;
                }


                if (filecontent.Contains("loadModuleDisplay:false"))
                {
                    loadModuleDisplay = false;
                }

                if (filecontent.Contains("registryApplyDisplay:false"))
                {
                    registryApplyDisplay = false;
                }


                if (filecontent.Contains("realtimePriority:false"))
                {
                    myProcess.PriorityClass = ProcessPriorityClass.Normal;
                }
                else
                {
                    myProcess.PriorityClass = ProcessPriorityClass.RealTime;
                }
                return;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("New config.yml file available");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Please press enter to recreate file..\n(Don't forget to copy your license key.)");
                string temp = Console.ReadLine();
                File.Delete(path.Replace("/config", "/conf.yml"));
                File.WriteAllText(path.Replace("/config", "/conf.yml"), "version:" + configVer + "\nlicenseKey:\n\n#Value: true/false\ndisplayError:true\ndisplayBuild:false\nloadModuleDisplay:true\nregistryApplyDisplay:true\nrealtimePriority:true");
                myProcess.Kill();
            }

        }




        static bool Login()
        {
            string authkey = "";
            string password = "";
            string username = "";


            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Please enter username: \n");
            Console.ForegroundColor = ConsoleColor.White;
            username = Console.ReadLine();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Please enter password: \n");
            Console.ForegroundColor = ConsoleColor.White;
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Substring(0, (password.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);


            Console.Clear();

            authkey = PersoCrypt(username, password);

            authkey = string.Concat(authkey.Length, authkey);
            authkey = EncryptUsingMD5(authkey);

            string filecontent = null;

            filecontent = File.ReadAllText(path.Replace("/config", "/conf.yml"));
            if (filecontent.Contains("licenseKey:" + authkey))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Valid license key.");
                Console.ForegroundColor = ConsoleColor.White;
                Sleep(1000);
                Console.Clear();
                Logs("Valid license key.");
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid license key.");
                Console.ForegroundColor = ConsoleColor.White;
                Sleep(1000);
                Console.Clear();
                Logs("Invalid license key.");
                return false;
            }
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





        static void Logs(string content)
        {
            StreamWriter file = new StreamWriter(path.Replace("/config", "/logs.txt"), true);
            file.Write("\n" + content);
            file.Close();
        }

        static void ErrorMsg(string error, string at)
        {
            if (displayError == true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[ERROR] " + error + "\nCode : " + at + "\n");
                Console.ForegroundColor = ConsoleColor.White;
                Logs("[ERROR] " + error + "\nCode : " + at);
                Sleep(1500);
            }
        }

        static bool EmptyLine(string find)
        {
            if (find != "" && find != Environment.NewLine + "\t" && find != Environment.NewLine + "\t\t" && find != Environment.NewLine + "    " && find != Environment.NewLine + "        ")
            {
                return true;
            }
            return false;
        }

        static void Sleep(int ms)
        {
            Thread.Sleep(ms);
        }

        static void Banner()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(" __      __   ____  ____  ___    ___  _____  ____  ____  _    _    __    ____  ____ \n(  )    /__\\ (_  _)(_  _)/ __)  / __)(  _  )( ___)(_  _)( \\/\\/ )  /__\\  (  _ \\( ___)\n )(__  /(__)\\  )(   _)(_ \\__ \\  \\__ \\ )(_)(  )__)   )(   )    (  /(__)\\  )   / )__) \n(____)(__)(__)(__) (____)(___/  (___/(_____)(__)   (__) (__/\\__)(__)(__)(_)\\_)(____)");
            Console.ForegroundColor = ConsoleColor.White;
            Sleep(3000);
            Console.Clear();
        }

        static bool IsNumeric(string Nombre)
        {
            {
                try
                {
                    int.Parse(Nombre);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        static void ClearLastLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }







        static void MainMenu()
        {
            int moduleid;
            string security;

            Console.Clear();

            if (configActive == true)
            {
                Console.WriteLine("Modules loaded: \n\n");

                for (int i = 0; i <= module_name.Count() - 1; i++)
                {
                    Sleep(200);
                    if(i == module_name.Count() - 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(module_name[i] + "\n\n");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(module_name[i]);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(", ");
                    }
                }
                Sleep(1000);


                Console.WriteLine("Reload [0]\n" + "Reload custom configs & conf.yml file\n\n");
                Console.WriteLine("Windows 10 [1]\n" + "Optimize Windows 10\n\n");
                Console.WriteLine("Game Opimize [2]\n" + "Optimize games for best reactivness and fps boost /!\\ Disable GameDVR\n\n");
                Console.WriteLine("Connection Opimize [3]\n" + "Optimize connection\n\n");

                for (int i = 0; i <= module_file.Count() - 1; i++)
                {
                    Console.WriteLine(module_list[i] + " [" + module_num[i].ToString() + "]" + "\n" + FindDesc(module_file[i], module_list[i]) + "\n\n");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No config files.\n\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Reload [0]\n" + "Reload custom configs & conf.yml file\n\n");
                Console.WriteLine("Windows 10 [1]\n" + "Optimize Windows 10\n\n");
                Console.WriteLine("Game Opimize [2]\n" + "Optimize games for best reactivness and fps boost /!\\ Disable GameDVR\n\n");
                Console.WriteLine("Connection Opimize [3]\n" + "Optimize connection\n\n");
            }

            Console.WriteLine("Please enter the number of which module to load :\n");

            security = Console.ReadLine();
            while (security == null || security == "" || IsNumeric(security) != true)
            {
                ClearLastLine();
                if (invalidInput == true)
                {
                    ClearLastLine();
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input.");
                Console.ForegroundColor = ConsoleColor.White;
                security = Console.ReadLine();
                invalidInput = true;
            }
            moduleid = int.Parse(security);

            if (moduleid <= totalModule && moduleid >= 0)
            {
                if (moduleid > moduleNum)
                {

                    int i = 0;

                    while (module_num[i] != moduleid)
                    {
                        i++;
                    }

                    Console.Clear();
                    Console.WriteLine("Running : " + module_list[i] + "..");
                    Sleep(1000);
                    Console.Clear();
                    invalidInput = false;
                    LoadModule(module_file[i], module_list[i]);
                }
                else
                {
                    Console.Clear();
                    invalidInput = false;
                    LoadHardModule(moduleid);
                }
            }
            else
            {
                Console.Clear();
                ErrorMsg("Can't load module.", "#0009");
                Console.Clear();
            }
        }






        static void LoadModule(string file, string module)
        {
            Logs("Launching custom module: " + module + " : " + file);

            string filecontent = File.ReadAllText(file);
            string[] lines = filecontent.Split('"');

            int num = 0;
            int modulenum = 1000;

            int see = 0;

            string regpath = "none";
            string key = "none";
            string value = "none";

            RegistryValueKind type = RegistryValueKind.None;

            foreach (string line in lines)
            {
                string newline = line.ToString();

                if (modulenum != 1000 && newline == "\n  " && newline == "\n\t")
                {
                    return;
                }




                num++;




                if (newline == module && see == 0)
                {
                    see++;
                }

                if (newline == module && see == 1)
                {
                    modulenum = num;
                }





                if (num == modulenum + 2 && newline.Contains("HKEY"))
                {

                    if (newline.Contains("HKEY_LOCAL_MACHINE") || newline.Contains("HKLM"))
                    {
                        newline = newline.Replace("HKLM", "HKEY_LOCAL_MACHINE");
                        regpath = newline;
                    }

                    else if (newline.Contains("HKEY_CURRENT_USER") || newline.Contains("HKCU"))
                    {
                        newline = newline.Replace("HKCU", "HKEY_CURRENT_USER");
                        regpath = newline;
                    }

                    else if (newline.Contains("HKEY_CLASSES_ROOT") || newline.Contains("HKCR"))
                    {
                        newline = newline.Replace("HKCR", "HKEY_CLASSES_ROOT");
                        regpath = newline;
                    }

                    else if (newline.Contains("HKEY_USERS") || newline.Contains("HKU"))
                    {
                        newline = newline.Replace("HKU", "HKEY_USERS");
                        regpath = newline;
                    }

                    else if (newline.Contains("HKEY_CURRENT_CONFIG") || newline.Contains("HKCC"))
                    {
                        newline = newline.Replace("HKCC", "HKEY_CURRENT_CONFIG");
                        regpath = newline;
                    }

                    else
                    {
                        ErrorMsg("Can't find HKEY.", "#0010");
                    }
                }




                if (num == modulenum + 4 && regpath != "none")
                {
                    if (newline != "")
                    {
                        key = newline;
                    }
                }



                if (num == modulenum + 6 && regpath != "none")
                {
                    if (newline != "")
                    {
                        newline = newline.ToUpper();
                        if (newline == "REG_DWORD")
                        {
                            type = RegistryValueKind.DWord;
                        }
                        else if (newline.ToUpper() == "REG_BINARY" || newline.ToUpper() == "BINARY" || newline.ToUpper() == "BIN")
                        {
                            type = RegistryValueKind.Binary;
                        }
                        else if (newline.ToUpper() == "REG_EXPAND_SZ" || newline.ToUpper() == "EXPAND_SZ")
                        {
                            type = RegistryValueKind.ExpandString;
                        }
                        else if (newline.ToUpper() == "REG_MULTI_SZ" || newline.ToUpper() == "MULTI_SZ")
                        {
                            type = RegistryValueKind.MultiString;
                        }
                        else if (newline.ToUpper() == "REG_QWORD" || newline.ToUpper() == "QWORD")
                        {
                            type = RegistryValueKind.QWord;
                        }
                        else if (newline.ToUpper() == "REG_SZ" || newline.ToUpper() == "SZ")
                        {
                            type = RegistryValueKind.String;
                        }
                        else
                        {
                            ErrorMsg("Can't resolve key type.", "#0011");
                        }
                    }
                }



                if (num == modulenum + 8 && regpath != "none")
                {
                    if (newline != "")
                    {
                        value = newline;
                        if (regpath != "none" && key != "none" && type != RegistryValueKind.None && value != "none")
                        {
                            Console.WriteLine("\nPath: " + regpath + "\nKey: " + key + "\nType: " + type + "\nValue: " + value + "\n");
                            RegistryChange(regpath, key, type, value);

                            modulenum = num;
                            regpath = "none";
                            key = "none";
                            value = "none";
                            type = RegistryValueKind.None;

                        }
                        else
                        {
                            ErrorMsg("Registry change info not complete.", "#0012");

                            regpath = "none";
                            key = "none";
                            value = "none";
                            type = RegistryValueKind.None;
                        }
                    }
                }
            }
        }

        static void LoadHardModule(int num)
        {
            Logs("Launching HardModule: " + num);
            if (num == 0)
            {
                displayError = true;
                loadModuleDisplay = true;
                registryApplyDisplay = true;

                myProcess.PriorityClass = ProcessPriorityClass.Normal;

                Console.Title = "Latis R.Engine " + version;
                string filecontent = null;

                try
                {
                    filecontent = File.ReadAllText(path.Replace("/config", "/conf.yml"));
                }
                catch (FileNotFoundException ex)
                {
                    if (ex.GetType().Name.Contains("FileNotFoundException"))
                    {
                        ErrorMsg("conf.yml doesn't exist", "#0014");
                        return;
                    }
                }

                if (filecontent.Contains("displayError:false"))
                {
                    displayError = false;
                }


                if (filecontent.Contains("displayBuild:true"))
                {
                    Console.Title = "Latis R.Engine " + version + " - BUILD: " + build;
                }


                if (filecontent.Contains("loadModuleDisplay:false"))
                {
                    loadModuleDisplay = false;
                }

                if (filecontent.Contains("registryApplyDisplay:false"))
                {
                    registryApplyDisplay = false;
                }


                if (filecontent.Contains("realtimePriority:false"))
                {
                    myProcess.PriorityClass = ProcessPriorityClass.Normal;
                }
                else
                {
                    myProcess.PriorityClass = ProcessPriorityClass.RealTime;
                }

                module_file.Clear();
                module_list.Clear();
                module_name.Clear();
                module_num.Clear();

                config_num = 0 + moduleNum;
                totalModule = 0 + moduleNum;

                configActive = true;
                InitConfig();
            }





            else if (num == 1)
            {
                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Memory Management", "LargeSystemCache", RegistryValueKind.DWord, "00000000");
                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile", "NetworkThrottlingIndex", RegistryValueKind.DWord, "00000192");
                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Memory Management", "SecondLevelDataCache", RegistryValueKind.DWord, "00000000");
                
                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile", "NetworkThrottlingIndex", RegistryValueKind.DWord, "00000012");
                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile", "SystemResponsiveness", RegistryValueKind.DWord, "00000000");

                SilentRegistryChange("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\Current\\Version\\Explorer\\Serialize", "StartupDelayInMSec", RegistryValueKind.DWord, "00000000");

                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection", "AllowTelemetry", RegistryValueKind.DWord, "00000000");

                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\WMI\\AutoLogger\\AutoLogger-Diagtrack-Listener", "Start", RegistryValueKind.DWord, "00000000");
                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\WMI\\AutoLogger\\SQMLogger", "Start", RegistryValueKind.DWord, "00000000");

                SilentRegistryChange("HKEY_CURRENT_USER\\Control Panel\\Desktop", "AutoEndTasks", RegistryValueKind.String, "1");
                SilentRegistryChange("HKEY_CURRENT_USER\\Control Panel\\Desktop", "MenuShowDelay", RegistryValueKind.String, "0");
                SilentRegistryChange("HKEY_CURRENT_USER\\Control Panel\\Desktop", "WaitToKillAppTimeout", RegistryValueKind.String, "5000");
                SilentRegistryChange("HKEY_CURRENT_USER\\Control Panel\\Desktop", "WaitToKillServiceTimeout", RegistryValueKind.String, "1000");
                SilentRegistryChange("HKEY_CURRENT_USER\\Control Panel\\Desktop", "HungAppTimeout", RegistryValueKind.String, "4000");
                SilentRegistryChange("HKEY_CURRENT_USER\\Control Panel\\Desktop", "LowLevelHooksTimeout", RegistryValueKind.String, "1000");
                SilentRegistryChange("HKEY_CURRENT_USER\\Control Panel\\Desktop", "ForegroundLockTimeout", RegistryValueKind.String, "150000");
            }
            else if (num == 2)
            {
                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile\\Tasks\\Games", "Affinity", RegistryValueKind.DWord, "00000000");
                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile\\Tasks\\Games", "Clock Rate", RegistryValueKind.DWord, "00002710");
                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile\\Tasks\\Games", "GPU Priority", RegistryValueKind.DWord, "00000008");
                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile\\Tasks\\Games", "Priority", RegistryValueKind.DWord, "00000006");
                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile\\Tasks\\Games", "Background Only", RegistryValueKind.String, "False");
                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile\\Tasks\\Games", "Scheduling Category", RegistryValueKind.String, "High");
                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile\\Tasks\\Games", "SFIO Priority", RegistryValueKind.String, "High");

                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\GraphicsDrivers\\Scheduler", "VsyncIdleTimeout", RegistryValueKind.DWord, "00000000");

                SilentRegistryChange("HKEY_CURRENT_USER\\System\\GameConfigStore", "GameDVR_Enabled", RegistryValueKind.DWord, "00000000");
                SilentRegistryChange("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\GameDVR", "AppCaptureEnabled", RegistryValueKind.DWord, "00000000");
                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\GameDVR", "AllowgameDVR", RegistryValueKind.DWord, "00000000");

                SilentRegistryChange("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Games", "FpsStatusGames", RegistryValueKind.DWord, "00000016");
                SilentRegistryChange("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Games", "FpsStatusGamesAll", RegistryValueKind.DWord, "00000004");
                SilentRegistryChange("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Games", "FpsAll", RegistryValueKind.DWord, "00000001");
                SilentRegistryChange("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Games", "GameFluidity", RegistryValueKind.DWord, "00000008");

                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Control\\PriorityControl", "Win32PrioritySeparation", RegistryValueKind.DWord, "00000026");
                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Control\\PriorityControl", "IRQ8Priority", RegistryValueKind.DWord, "00000001");
            }
            else if (num == 3)
            {
                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\Psched", "NonBestEffortLimit", RegistryValueKind.DWord, "00000000");
                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters", "DefaultTTL", RegistryValueKind.DWord, "00000140");
                SilentRegistryChange("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\MSMQ\\Parameters", "TCPNoDelay", RegistryValueKind.DWord, "00000001");

                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    SilentRegistryChange("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters\\Interfaces\\" + nic.Id, "TcpAckFrequency", RegistryValueKind.DWord, "00000000");
                    SilentRegistryChange("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters\\Interfaces\\" + nic.Id, "TcpDelAckTicks", RegistryValueKind.DWord, "00000001");
                    SilentRegistryChange("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters\\Interfaces\\" + nic.Id, "TCPNoDelay", RegistryValueKind.DWord, "00000001");
                }

            }
        }












        static void RegistryChange(string path, string name, RegistryValueKind type, string value)
        {
            string oldValue = null;

            

            if (Registry.GetValue(path, name, "none") != null)
            {
                oldValue = Registry.GetValue(path, name, "none").ToString();
            }


            if (oldValue == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Subkey not found creating it..");
                Console.WriteLine("Value not found creating it..");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nTrying to parse: null -> " + value);
                Console.ForegroundColor = ConsoleColor.White;
                Logs("Creating subkey & value & parsing it: " + path + " : " + name + " : " + value);
            }
            else if (oldValue == "none")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Value not found creating it..");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nTrying to parse: null -> " + value);
                Console.ForegroundColor = ConsoleColor.White;
                Logs("Creating value & parsing it: " + name + " : " + value);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nTrying to parse: " + oldValue + " -> " + value);
                Console.ForegroundColor = ConsoleColor.White;
                Logs("Parsing value: " + name + " : " + value);
            }

            try
            {

                if (type == RegistryValueKind.Binary)
                {
                    byte[] hex = value.Split(',')
                              .Select(x => Convert.ToByte(x, 16))
                              .ToArray();
                    Logs("Hex parse done.");
                    Registry.SetValue(path, name, hex, type);
                    Logs("Value set: '" + path + " : " + name + " : " + value + " : " + type + "'");
                }


                else if (type == RegistryValueKind.MultiString)
                {
                    string[] multi = value.Split(',');
                    Logs("Multi parse done.");
                    Registry.SetValue(path, name, multi, type);
                    Logs("Value set: '" + path + " : " + name + " : " + value + " : " + type + "'");
                }

                else

                {
                    Registry.SetValue(path, name, value, type);
                    Logs("Value set: '" + path + " : " + name + " : " + value + " : " + type + "'");
                }
            }


            catch (ArgumentException ex)

            {
                if (ex.GetType().Name.Contains("ArgumentException"))
                {
                    ErrorMsg("Wrong value type.", "#0013");
                }
            }
            if (registryApplyDisplay == true)
            {
                Sleep(1000);
            }
        }

        static void SilentRegistryChange(string path, string name, RegistryValueKind type, string value)
        {
            Registry.SetValue(path, name, value, type);
            Logs("Hard value set.");
        }










        static void ModuleAdd(string file, string list, int num)
        {
            module_file.Add(file);
            module_list.Add(list);
            module_num.Add(num);
            Logs("Adding custom configs module informations : '" + file + " " + list + " " + num + "'");
        }

        static string FindDesc(string file, string module)
        {
            string filecontent = File.ReadAllText(file);
            string[] lines = filecontent.Split('"');

            int num = 0;
            int modulenum = 10000;

            foreach (string line in lines)
            {
                string newline = line.ToString();

                num++;

                if (newline == module)
                {
                    modulenum = num;
                }
                else if (num == modulenum + 2)
                {
                    return newline;
                }

            }
            Logs("Can't find description for custom modules: " + module + " in file " + file);
            return null;
        }


        static void ConfigDetect(string file)
        {
            int num = 0;

            string name = null;
            string version = null;
            string author = null;

            string filecontent = File.ReadAllText(file);
            string[] lines = filecontent.Split('"');

            foreach (string line in lines)
            {
                string newline = line.ToString();

                num++;


                if (num == 2 && newline != "name")
                {
                    ErrorMsg("Unable to find config name, aborting..", "#0002");
                    return;
                }


                else if (num == 4)
                {
                    name = newline;
                    if (EmptyLine(newline) == false)
                    {
                        ErrorMsg("Config name not valid.", "#0003");
                        return;
                    }
                    else
                    {
                        if (loadModuleDisplay == true)
                        {
                            Console.WriteLine("Name: " + newline);
                            Sleep(500);
                        }
                    }
                }





                else if (num == 6 && newline != "version")
                {
                    ErrorMsg("Unable to find config version, aborting..", "#0004");
                    return;
                }

                else if (num == 8)
                {
                    version = newline;
                    if (EmptyLine(newline) == false)
                    {
                        ErrorMsg("Config version not valid", "#0005");
                        return;
                    }
                    else
                    {
                        if (loadModuleDisplay == true)
                        {
                            Console.WriteLine("Version: " + newline);
                            Sleep(500);
                        }
                    }
                }





                else if (num == 10 && newline != "author")
                {
                    ErrorMsg("Unable to find config author, aborting..", "#0006");
                    return;
                }

                else if (num == 12)
                {
                    author = newline;
                    if (EmptyLine(newline) == false)
                    {
                        ErrorMsg("Config author not valid", "#0007");
                        return;
                    }
                    else
                    {
                        if (loadModuleDisplay == true)
                        {
                            Console.WriteLine("Author: " + newline);
                            Sleep(500);
                        }
                    }
                }



                else if (num == 14 && newline != "modules")
                {
                    ErrorMsg("Unable to find config modules list, aborting..", "#0008");
                    return;
                }
                else if (num == 14)
                {
                    Console.Clear();
                    Console.WriteLine("Looking for modules..\n\n");
                }

                if (num > 15)
                {

                    if (newline != Environment.NewLine + "    " && newline != Environment.NewLine + "\t")
                    {
                        if (num % 4 == 0)
                        {
                            if (loadModuleDisplay == true)
                            {
                                Console.WriteLine("Module found in " + name + ": " + newline);
                                config_num++;
                                ModuleAdd(file, newline, config_num);
                                totalModule++;
                                Sleep(250);
                            }
                            else
                            {
                                config_num++;
                                ModuleAdd(file, newline, config_num);
                                totalModule++;
                            }
                        }
                    }
                    else
                    {
                        Console.Clear();
                        module_name.Add(name + " " + version + " by " + author);
                        Logs("Adding custom configs informations : '" + name + " " + version + " by " + author + "'");
                        return;
                    }
                }

            }
        }


        static void InitConfig()
        {
            int filenum = 0;

            if (Directory.Exists(path))
            {
                Console.WriteLine("\nLooking for config files..\n\n");
                Logs("Looking for config files..");
                Sleep(1000);

                DirectoryInfo dir = new DirectoryInfo(@path);
                FileInfo[] fichiers = dir.GetFiles();

                foreach (FileInfo fichier in fichiers)
                {
                    if(fichier.Name.Contains(".yml"))
                    {
                        filenum++;
                        ConfigDetect(path + "/" + fichier.Name);
                    }
                }
                if (filenum == 0)
                {
                    configActive = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No config files.\n\n");
                    Logs("No config files.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Sleep(1500);
                    Console.Clear();
                }
            }
            else
            {
                Console.WriteLine("No config directory, attempts to create one..");
                Logs("No config directory, attempts to create one..");
                Directory.CreateDirectory(path);
                if (Directory.Exists(path))
                {
                    {
                        Console.WriteLine("\nLooking for config files..\n\n");
                        Logs("Looking for config files..");
                        Sleep(1000);

                        DirectoryInfo dir = new DirectoryInfo(@path);
                        FileInfo[] fichiers = dir.GetFiles();

                        foreach (FileInfo fichier in fichiers)
                        {
                            if(fichier.Name.Contains(".yml"))
                            {
                                filenum++;
                                ConfigDetect(path + "/" + fichier.Name);
                            }
                        }
                        if (filenum == 0)
                        {
                            configActive = false;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("No config files.\n\n");
                            Logs("No config files.");
                            Console.ForegroundColor = ConsoleColor.White;
                            Sleep(1500);
                            Console.Clear();
                        }
                    }
                }
                else
                {
                    configActive = false;
                    ErrorMsg("Can't create config directory.", "#0001");
                }
            }
        }
    }
}
