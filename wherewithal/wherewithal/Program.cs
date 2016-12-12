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
    public static class ProcessoAtivo
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
                    Console.WriteLine(_processo.ToString() + " - " + ProcessoNome + " - " + BrowserURL);

                    //System.IO.File.AppendAllText(@"C:\Users\Ivan\Desktop\new 4.txt", _processo.ToString() + " - " + ProcessoNome + " - " + BrowserURL + '\n');
                }

            }
        }
        public static string ProcessoNome { get; set; }
        public static string BrowserURL { get; set; }

        private static string BrowserURLUltimoGravado { get; set; }


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
            while (true)
            {

                hWnd = GetForegroundWindow();
                procId = 0;
                GetWindowThreadProcessId(hWnd, out procId);
                proc = Process.GetProcessById((int)procId);

                //Console.WriteLine(proc.MainModule);
                if (proc.ProcessName.Equals("firefox") || proc.ProcessName.Equals("chrome"))
                {
                    url = GetChromeUrl(proc);
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

                //task.Start();
                //Console.ReadKey();
                System.Threading.Thread.Sleep(500); // Test it with 5 Seconds, set a window to foreground, and you see it works!
            }

        }
        public static async Task atualizaProcesso(uint processoID = 0, string processoNome = "", string browserURL = "")
        {
            ProcessoAtivo.ProcessoNome = processoNome;
            ProcessoAtivo.BrowserURL = browserURL;
            ProcessoAtivo.ProcessoID = processoID;
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