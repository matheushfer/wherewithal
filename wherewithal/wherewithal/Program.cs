using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Timers;
using Harvest;
using System.ComponentModel;
using SolutionLibrary.Classes;

namespace monitor
{
    //class Program
    //{
    //    [DllImport("user32.dll")]
    //    private static extern IntPtr GetForegroundWindow();
    //    static void Main(string[] args)
    //    {
    //        while (true)
    //        {
    //            Console.WriteLine("Internet Explorer: ");

    //            (new List<browserlocation.URLDetails>(browserlocation.InternetExplorer())).ForEach(u =>
    //            {
    //                Console.WriteLine("[{0}]\r\n{1}\r\n", u.Title, u.URL);
    //            });

    //            /*Console.Write("\n press enter to view Chrome current tab URL");
    //            Console.ReadLine();

    //            foreach (Process process in Process.GetProcessesByName("chrome"))
    //            {
    //                string url = browserlocation.GetChromeUrl(process);
    //                if (url == null)
    //                    continue;

    //                Console.WriteLine("CH Url for '" + process.MainWindowTitle + "' is " + url);
    //                //Console.ReadLine();
    //                Process currentp = Process.GetCurrentProcess();
    //                //Console.WriteLine("Processo atual é: " + currentp.ToString());

    //            }
    //            foreach (Process process in Process.GetProcessesByName("firefox"))
    //            {
    //                string url = browserlocation.GetChromeUrl(process);
    //                if (url == null)
    //                    continue;

    //                Console.WriteLine("FIREFOX Url for '" + process.MainWindowTitle + "' is " + url);
    //                //Console.ReadLine();
    //                Process currentp = Process.GetCurrentProcess();
    //                //Console.WriteLine("Processo atual é: " + currentp.ToString());

    //            }*/




    //            IntPtr selectedWindow = GetForegroundWindow();
    //            Console.WriteLine(selectedWindow);
    //            Process processAtual = Process.GetCurrentProcess();

    //            //Console.WriteLine(processAtual.ToString());
    //            if (processAtual.ProcessName.Equals("firefox") || processAtual.ProcessName.Equals("chrome"))
    //            {
    //                string url = browserlocation.GetChromeUrl(processAtual);
    //                if (url == null)
    //                    continue;
    //                Console.WriteLine(processAtual.ProcessName + " Url for '" + processAtual.MainWindowTitle + "' is " + url);
    //            }




    //        }




    //    }

    //    /*public ApplicationState AppState
    //    {
    //        get
    //        {
    //            Process[] processCollection =
    //                               Process.GetProcessesByName(ProcessName);
    //            if (processCollection != null &&
    //               processCollection.Length >= 1 &&
    //                processCollection[0] != null)
    //            {
    //                IntPtr activeWindowHandle = Win32.GetForegroundWindow();
    //                // Optional int ProcessID;
    //                // Optional Win32.GetWindowThreadProcessId(GetForegroundWindow(), out ProcessID)
    //        foreach (Process wordProcess in processCollection)
    //                {
    //                    //Optional if( ProcessID == wordProcess.Id )
    //                    //          return ApplicationState.Focused;
    //                    if (wordProcess.MainWindowHandle == activeWindowHandle)
    //                    {
    //                        return ApplicationState.Focused;
    //                    }
    //                }

    //                return ApplicationState.Running;
    //            }

    //            return ApplicationState.NotRunning;
    //        }
    //    }*/
    //}

    /*class browserlocation
    {
        public struct URLDetails
        {
            public String URL;
            public String Title;
        }
        public static URLDetails[] InternetExplorer()
        {
            Process[] pname = Process.GetProcessesByName("iexplore");
            if (pname.Length == 0)
            {
                Console.Write("Process is not running ");
            }

            System.Collections.Generic.List<URLDetails> URLs = new System.Collections.Generic.List<URLDetails>();
            var shellWindows = new SHDocVw.ShellWindows();
            foreach (SHDocVw.InternetExplorer ie in shellWindows)
                URLs.Add(new URLDetails() { URL = ie.LocationURL, Title = ie.LocationName });
            return URLs.ToArray();
        }
        public static string GetChromeUrl(Process process)
        {
            if (process == null)
                throw new ArgumentNullException("process");

            if (process.MainWindowHandle == IntPtr.Zero)
                return null;

            AutomationElement element = AutomationElement.FromHandle(process.MainWindowHandle);
            if (element == null)
                return null;

            AutomationElement edit = element.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
            return ((ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;


        }
    }*/
    public static class Monitor
    {
        /*private uint _processoID;
        public uint ProcessoID
        {
            get
            {
                return _processoID;
            }
            set
            {
                if (value != _processoID)
                {
                    _processoID = value;
                }
            }
        }*/
        private static uint _processo;
        public static uint ProcessoID
        {
            get
            {
                return _processo;
            }
            set
            {
                if (_processo != value || BrowserURL != BrowserURLUltimoGravado)
                {
                    _processo = value;
                    BrowserURLUltimoGravado = BrowserURL;
                    Task t = new Task(() => Insert());
                    t.Start();
                    //Console.WriteLine(_processo.ToString() + " - " + ProcessoNome + " - " + BrowserURL);

                    //System.IO.File.AppendAllText(@"C:\Users\Ivan\Desktop\new 4.txt", _processo.ToString() + " - " + ProcessoNome + " - " + BrowserURL + '\n');
                }

            }
        }
        public static string ProcessoNome { get; set; }
        public static string BrowserURL { get; set; }
        private static string BrowserURLUltimoGravado { get; set; }
        public static ConexaoDB Database { get; set; }
        public static string NomeEstacao { get; set; }
        public static Boolean graveiStartBuild { get; set; }
        private static async void Insert()
        {
            string[] fields = { "estacao",
                                "processo",
                                "url"/*,
                                "datahorainicio"*/};
            string[] values = { "'" + NomeEstacao.Trim() + "'",
                                "'" + ProcessoNome.Trim() + "'",
                                "'" + BrowserURL.Trim() + "'"/*,
                                "'" + DateTime.Now.Day.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year.ToString() + " " + DateTime.Now.Hour.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Second.ToString().PadLeft(2, '0') + "'" */};
            //string where = "";
            Database.insert("atividade", fields, values);
        }



    }

    class Program
    {
        static void Main(string[] args)
        {
            uint procId = 0;
            var proc = Process.GetCurrentProcess();
            IntPtr hWnd = GetForegroundWindow();
            string url = string.Empty;
            var task = new Task(() => atualizaProcesso(procId, proc.ProcessName, url));
            Monitor.Database = new ConexaoDB("192.168.9.20", "5433", "postgres", "wherewithal", "wherewithal", true, MsgType.Msg);
            Monitor.NomeEstacao = Environment.MachineName;
            Monitor.graveiStartBuild = false;
            DateTime dataHoraBuildMaisVelho = DateTime.MaxValue;
            while (true)
            {
                //(new List<browserlocation.URLDetails>(browserlocation.InternetExplorer())).ForEach(u =>
                //            {
                //                Console.WriteLine("[{0}]\r\n{1}\r\n", u.Title, u.URL);
                //            });

                hWnd = GetForegroundWindow();
                procId = 0;
                GetWindowThreadProcessId(hWnd, out procId);
                proc = Process.GetProcessById((int)procId);

                //Console.WriteLine(proc.MainModule);
                if (proc.ProcessName.Equals("firefox") || proc.ProcessName.Equals("chrome")/* || proc.ProcessName.Equals("iexplore")*/) // IE está pegando os endereços de todas as abas, não apenas a que está em foco
                {
                    url = string.Empty;

                    if (proc.ProcessName.Equals("iexplore"))
                    {
                        (new List<URLDetails>(InternetExplorer())).ForEach(u =>
                        {
                            url += u.URL;
                            //Console.WriteLine("[{0}]\r\n{1}\r\n", u.Title, u.URL);
                        });
                    }
                    else
                    {
                        url = GetChromeUrl(proc);
                    }

                    if (url == null)
                        continue;
                    url = url.ToLower();
                    url = url.Replace("http://", string.Empty).Replace("https://", string.Empty).Replace("ftp://", string.Empty);
                    if (url.Contains("/"))
                    {
                        url = url.Substring(0, url.IndexOf("/"));
                    }

                    //Console.WriteLine(proc.ProcessName + " Url for '" + proc.MainWindowTitle + "' is " + url);
                }
                else
                {
                    url = string.Empty;
                }


                task = new Task(() => atualizaProcesso(procId, proc.ProcessName, url));
                task.Start();

                /*Process[] localByName = Process.GetProcessesByName("MSBuild");
                
                if (localByName.Count() > 0)
                {
                    if (Monitor.graveiStartBuild == false)
                    {
                        procId = 0;
                        foreach (Process item in localByName)
                        {
                            if (procId < item.Id)
                            {
                                procId = (uint)item.Id;
                                dataHoraBuildMaisVelho = item.StartTime;
                            }
                                
                        }
                        task = new Task(() => compileStart(localByName[0].ProcessName, dataHoraBuildMaisVelho));
                        task.Start();
                        Monitor.graveiStartBuild = true;
                    }
                }
                else
                {
                    if (Monitor.graveiStartBuild == true)
                    {
                        task = new Task(() => compileStop(DateTime.Now));
                        task.Start();
                        Monitor.graveiStartBuild = false;
                    }
                }*/


                //task.Start();
                //Console.ReadKey();
                System.Threading.Thread.Sleep(500); // Test it with 5 Seconds, set a window to foreground, and you see it works!
            }

        }
        public static async Task atualizaProcesso(uint processoID = 0, string processoNome = "", string browserURL = "")
        {
            Monitor.ProcessoNome = processoNome;
            Monitor.BrowserURL = browserURL;
            Monitor.ProcessoID = processoID;
        }

        public static async Task compileStart(string processoNome, DateTime datahora)
        {
            string[] fields = { "estacao",
                                "processo",
                                "datahorainicio"};
            string[] values = { "'" + Monitor.NomeEstacao.Trim() + "'",
                                "'" + processoNome.Trim() + "'",
                                "'" + datahora.Day.ToString().PadLeft(2, '0') + "/" + datahora.Month.ToString().PadLeft(2, '0') + "/" + datahora.Year.ToString() + " " + datahora.Hour.ToString().PadLeft(2, '0') + ":" + datahora.Minute.ToString().PadLeft(2, '0') + ":" + datahora.Second.ToString().PadLeft(2, '0') + "'" };
            //string where = "";
            Monitor.Database.insert("compilacao", fields, values);
        }
        public static async Task compileStop(DateTime datahora)
        {
            string[] fields = {"datahorafim"};
            string[] values = { "'" + datahora.Day.ToString().PadLeft(2, '0') + "/" + datahora.Month.ToString().PadLeft(2, '0') + "/" + datahora.Year.ToString() + " " + datahora.Hour.ToString().PadLeft(2, '0') + ":" + datahora.Minute.ToString().PadLeft(2, '0') + ":" + datahora.Second.ToString().PadLeft(2, '0') + "'" };
            string where = "estacao = " + Monitor.NomeEstacao.Trim();
            Monitor.Database.update("compilacao", fields, values, where);
        }
        public struct URLDetails
        {
            public String URL;
            public String Title;
        }
        public static URLDetails[] InternetExplorer()
        {
            Process[] pname = Process.GetProcessesByName("iexplore");
            if (pname.Length == 0)
            {
                return null;
            }

            System.Collections.Generic.List<URLDetails> URLs = new System.Collections.Generic.List<URLDetails>();
            var shellWindows = new SHDocVw.ShellWindows();
            foreach (SHDocVw.InternetExplorer ie in shellWindows)
            {
                
                if (ie.Name.Contains("Internet Explorer"))
                {
                    //Console.WriteLine("");
                    //Console.WriteLine("IE:");
                    //Console.WriteLine("LocationName:" + ie.LocationName);
                    //Console.WriteLine("LocationURL:" + ie.LocationURL);
                    //Console.WriteLine("AddressBar:" + ie.AddressBar);
                    //Console.WriteLine("Application:" + ie.Application);
                    //Console.WriteLine("Document:" + ie.Document);
                    //Console.WriteLine("FullName:" + ie.FullName);
                    //Console.WriteLine("Name:" + ie.Name);
                    //Console.WriteLine("Parent:" + ie.Parent);
                    //Console.WriteLine("Path:" + ie.Path);
                    //Console.WriteLine("Visible:" + ie.Visible);
                    URLs.Add(new URLDetails() { URL = ie.LocationURL, Title = ie.LocationName });
                }
                
            }
                
            return URLs.ToArray();
        }
        public static string GetChromeUrl(Process process)
        {
            if (process == null)
                throw new ArgumentNullException("process");

            if (process.MainWindowHandle == IntPtr.Zero)
                return null;

            AutomationElement element = AutomationElement.FromHandle(process.MainWindowHandle);
            if (element == null)
                return null;

            AutomationElement edit = element.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
            if (edit != null)
            {
                return ((ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
            }
            else
            {
                return string.Empty;

            }




        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
    }
}