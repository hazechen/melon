using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;

namespace Melon
{
    class Brain
    {
        #region init
        DataTable dtMemory = new DataTable();
        string memoryPath = @"C:\Temp\Memories.xml";
        string tempPath = @"C:\Temp\";
        string type = "";
        string source = "";
        string input = "";
        public void wakeUp()
        {
            startThinking();
            importDt();
            //Console.SetWindowSize(80, 20);
            Console.WriteLine("Melon: Hello. This is Melon.");
        }
        public void startThinking()
        {
            dtMemory.Clear();
            dtMemory.TableName = "memories";
            dtMemory.Columns.Add("input");
            dtMemory.Columns.Add("type");
            dtMemory.Columns.Add("source");
        }
        public void importDt()
        {
            if (Directory.Exists(tempPath))
            {
                if (File.Exists(memoryPath))
                {
                    dtMemory.ReadXml(memoryPath);
                }
            }
            else
            {
                Directory.CreateDirectory(tempPath);
                createNewMemory();
                exportDt();
            }
        }
        public void exportDt()
        {
            dtMemory.WriteXml(memoryPath);
        }
        public void createNewMemory()
        {
            DataRow row = dtMemory.NewRow();
            row["input"] = "chrome";
            row["type"] = "process";
            row["source"] = "chrome.exe";
            dtMemory.Rows.Add(row);
        }
        #endregion init
        #region input
        public void getInput(string input)
        {
            this.input = input;
            processInput(input);
        }
        public void processInput(string input)
        {
            try
            {
                if (
                    input.StartsWith("start ") || input.StartsWith("open ") || input.StartsWith("o ") || input.StartsWith("s "))
                {
                    input = input.Substring(input.IndexOf(" ") + 1);
                    if (input.StartsWith("service "))
                    {
                        input = input.Substring(input.IndexOf(" ") + 1);
                        if (searchMemory(input))
                        {
                            startService(source);
                        }
                        else
                        {
                            startService(input);
                        }
                    }
                    else
                    {
                        input = input.Substring(input.IndexOf(" ") + 1);
                        if (input == "")
                        {
                            Console.WriteLine("Melon: what???");
                        }
                        else
                        {
                            decide(input);
                        }
                    }
                }
                else if (input.StartsWith("stop "))
                {
                    input = input.Substring(input.IndexOf(" ") + 1);
                    if (input.StartsWith("service "))
                    {

                        input = input.Substring(input.IndexOf(" ") + 1);
                        if (searchMemory(input))
                        {
                            stopService(source);
                        }
                        else
                        {
                            stopService(input);
                        }
                    }
                }
                else if (input.StartsWith("restart service "))
                {
                    input = input.Substring(input.IndexOf(" ") + 1);
                    input = input.Substring(input.IndexOf(" ") + 1);
                    if (searchMemory(input))
                    {
                        restartService(source);
                    }
                    else
                    {
                        restartService(input);
                    }
                }
                else if (input.StartsWith("check "))
                {
                    input = input.Substring(input.IndexOf(" ") + 1);

                    if (input.Contains("service status "))
                    {
                        input = input.Substring(input.IndexOf(" ") + 1);
                        input = input.Substring(input.IndexOf(" ") + 1);

                        string status = "";
                        if (searchMemory(input))
                        {
                            status = checkStatusService(source);
                        }
                        else
                        {
                            status = checkStatusService(input);
                        }
                        Console.WriteLine("Melon: This is the current status of this Service: " + status);
                    }
                }

                else if (input.StartsWith("search procedure for "))
                {
                    string search = input.Substring(input.LastIndexOf(" ") + 1);
                    if (search == "")
                    {
                        Console.WriteLine("Melon: Just tell me what I should search for.");
                    }
                    else
                    {
                        string procedure = "https://aisweb.ais.de/aisprocedure_reloaded/index.php?search=" + search;
                        Process.Start("chrome.exe", procedure);
                    }
                }
                else if (input.StartsWith("search for "))
                {
                    string search = input.Substring(input.IndexOf(" "));
                    search = input.Substring(input.IndexOf(" ") + 1);
                    if (search == "")
                    {
                        Console.WriteLine("Melon: Search for what???");
                    }
                    else
                    {
                        string google = "\"https://www.google.de/search?q=" + search + "\"";
                        Console.WriteLine("Melon: Googling " + search + " ...");
                        Process.Start("chrome.exe", google);
                    }
                }
                else if (input.StartsWith("search "))
                {
                    string search = input.Substring(input.IndexOf(" "));
                    if (search == "")
                    {
                        Console.WriteLine("Melon: Search for what???");
                    }
                    else
                    {
                        string google = "\"https://www.google.de/search?q=" + search + "\"";
                        Console.WriteLine("Melon: Googling " + search + " ...");
                        Process.Start("chrome.exe", google);
                    }
                }


                else if (input.StartsWith("procedure "))
                {
                    string search = input.Substring(input.IndexOf(" ") + 1);
                    if (search == "")
                    {
                        Console.WriteLine("Melon: Just tell me what I should search for.");
                    }
                    else
                    {
                        string procedure = "https://aisweb.ais.de/aisprocedure_reloaded/index.php?search=" + search;
                        Process.Start("chrome.exe", procedure);
                    }
                }
                else if (input.StartsWith("google "))
                {
                    string search = input.Substring(input.IndexOf(" ") + 1);
                    if (search == "")
                    {
                        Console.WriteLine("Melon: Just tell me what I should google.");
                    }
                    else
                    {
                        string google = "\"https://www.google.de/search?q=";
                        google = google + search + "\"";
                        Console.WriteLine("Melon: Googling " + search + " ...");
                        Process.Start("chrome.exe", google);
                    }
                }
                else if (input.StartsWith("google for "))
                {
                    string search = input.Substring(input.IndexOf(" ") + 1);
                    search = input.Substring(input.IndexOf(" ") + 1);
                    if (search == "")
                    {
                        Console.WriteLine("Melon: Just tell me what I should google.");
                    }
                    else
                    {
                        string google = "\"https://www.google.de/search?q=";
                        google = google + search + "\"";
                        Console.WriteLine("Melon: Googling " + search + " ...");
                        Process.Start("chrome.exe", google);
                    }
                }
                else if (input.StartsWith("wiki "))
                {
                    string search = input.Substring(input.IndexOf(" ") + 1);
                    if (search == "")
                    {
                        Console.WriteLine("Melon: Just tell me what I should search on Wikipedia.");
                    }
                    else
                    {
                        string wiki = "\"https://de.wikipedia.org/wiki/" + search + "\"";
                        Console.WriteLine("Melon: Okay! Looking up " + search + " on Wikipedia.");
                        Process.Start("chrome.exe", wiki);
                    }
                }

                else if (input.StartsWith("learn "))
                {
                    string learnThis = input.Substring(input.IndexOf(" ") + 1);
                    if (learnThis == "")
                    {
                        Console.WriteLine("Melon: Would be nice if you tell me what I should learn.");
                    }
                    else
                    {
                        learn(learnThis);
                    }
                }
                else if (input.StartsWith("help"))
                {
                    Console.WriteLine("Melon: I'll help you out.");
                    System.Threading.Thread.Sleep(500);
                    Console.WriteLine("Melon: Here is a list of commands I do understand:");
                    System.Threading.Thread.Sleep(500);
                    Console.WriteLine("Melon: start, s, open, o");
                    Console.WriteLine("Melon: start service, stop service, restart service");
                    Console.WriteLine("Melon: check service status");
                    Console.WriteLine("Melon: search, search for, search procedure for, procedure");
                    Console.WriteLine("Melon: google, google for, wiki");
                    System.Threading.Thread.Sleep(200);
                    Console.WriteLine("Melon: for example: \"start chrome\", or \"open notepad\"");
                    Console.WriteLine("Melon: \"google something\", \"wiki this\", \"procedure something\"");
                    System.Threading.Thread.Sleep(200);
                    Console.WriteLine("Melon: \"memory\" shows you all saved memories");
                    Console.WriteLine("Melon: \"remove\" deletes a memory");
                    Console.WriteLine("Melon: if you want to teach me something new just type: \"learn (name)\"");
                }
                else if (input.StartsWith("memory"))
                {
                    Console.WriteLine("Melon: Here's a list of my memory:");
                    foreach (DataRow row in dtMemory.Rows)
                    {
                        Console.WriteLine(row[0]);
                    }
                }
                else if (input.Contains("fuck you"))
                {
                    for (int i = 1; i < 40; i++)
                    {
                        Console.Clear();
                        Console.SetWindowSize(i, i);
                        System.Threading.Thread.Sleep(1);
                    }
                    Console.SetCursorPosition(10, 20);
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine(" F U C K  Y O U  T O O !");
                    System.Threading.Thread.Sleep(100);
                }
                else if (input.Contains("remove "))
                {   
                    string remove = input.Substring(input.IndexOf(" ") + 1);
                    if (remove == "")
                    {
                        Console.WriteLine("Melon: What should I remove?");
                    }
                    else
                    {
                        removeFromMemory(remove);
                    }            
                }
                else
                {
                    Console.WriteLine("Melon: I have no Idea what you're talking about.");
                    Console.WriteLine("Melon: Type \"help\" if you want to know more about what I can do.");
                }
            }
            catch
            {
                Console.WriteLine("Melon: I have no Idea what you're talking about.");
            }
        }
        public void decide(string input)
        {
            if (searchMemory(input))
            {
                if (type == "process")
                {
                    startProcess(source);
                }
                else if (type == "explorer")
                {
                    startExplorer(source);
                }
                else if (type == "browser")
                {
                    startBrowser(source);
                }
                else if (type == "service")
                {
                    Console.WriteLine("Melon: I think this is a service.");
                    Console.WriteLine("Melon: You should say start, stop or restart service :)");
                }
            }
            else
            {
                Console.WriteLine("Melon: I don't know what you mean by " + input);
                learn(input);
            }
        }
        #endregion input
        #region memory
        public bool searchMemory(string input)
        {
            bool found = false;
            foreach (DataRow row in dtMemory.Rows)
            {
                if (found)
                {
                    break;
                }
                if (row.ItemArray[0].ToString() == input)
                {
                    input = row.ItemArray[0].ToString();
                    type = row.ItemArray[1].ToString();
                    source = row.ItemArray[2].ToString();
                    found = true;
                }
            }
            return found;
        }
        public void addToMemory(string input, string type, string source)
        {
            DataRow row = dtMemory.NewRow();
            row["input"] = input;
            row["type"] = type;
            row["source"] = source;
            dtMemory.Rows.Add(row);
        }
        public void removeFromMemory(string input)
        {
            bool found = false;
            foreach (DataRow row in dtMemory.Rows)
            {
                if (row.ItemArray[0].ToString() == input)
                {
                    dtMemory.Rows.Remove(row);
                    found = true;
                    Console.WriteLine("Melon: Removed.");
                    exportDt();
                    break;
                }
            }
            if (!found)
            {
                Console.WriteLine("Melon: Couldn't find it in my memory.");
            }
        }

        public void learn(string input)
        {
            Console.WriteLine("Melon: Is this a Process(1), Service(2), Folder(3), Weblink(4)? or (5) to cancel.");
            string readKey = Console.ReadLine();

            if (readKey == "1")
            {
                Console.WriteLine("Melon: Please give me the source for this:");
                addToMemory(input, "process", Console.ReadLine());
                exportDt();
                Console.WriteLine("Melon: Thanks. I'll remember " + input + " from now on.");
            }
            else if (readKey == "2")
            {
                Console.WriteLine("Melon: Please give me the name of the service:");
                addToMemory(input, "service", Console.ReadLine());
                exportDt();
                Console.WriteLine("Melon: Thanks. I'll remember " + input + " from now on.");
            }
            else if (readKey == "3")
            {
                Console.WriteLine("Melon: Please give me the path for this:");
                addToMemory(input, "explorer", Console.ReadLine());
                exportDt();
                Console.WriteLine("Melon: Thanks. I'll remember " + input + " from now on.");
            }
            else if (readKey == "4")
            {
                Console.WriteLine("Melon: Please give me the link for this:");
                addToMemory(input, "browser", Console.ReadLine());
                exportDt();
                Console.WriteLine("Melon: Thanks. I'll remember " + input + " from now on.");
            }
            else if (readKey == "5")
            {
                Console.WriteLine("Melon: Ok. Canceled.");
            }
            else
            {
                Console.WriteLine("Melon: I didn't get it. Sorry");
            }
        }
        #endregion memory
        #region execute
        public void startProcess(string source)
        {
            Process.Start("\"" + source + "\"");
            Console.WriteLine("Melon: Done.");
        }
        public void startExplorer(string source)
        {
            System.Diagnostics.Process.Start("explorer.exe", source);
            Console.WriteLine("Melon: Sure! Done.");
        }
        public void startBrowser(string source)
        {
            Process.Start("chrome.exe", "\"" + source + "\"");
            Console.WriteLine("Melon: Sure! Opening Browser.");
        }

        //Service stuff
        public static void startService(string serviceName)
        {
            int timeoutMilliseconds = 3000;
            ServiceController service = new ServiceController(serviceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                Console.WriteLine("Melon: Trying to start " + serviceName + " service. Hold on.");
                service.Start();
                //Console.WriteLine("Melon: Waiting for " + timeout + " seconds...");
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                Console.WriteLine("Melon: Service is running now :)");
            }
            catch
            {
                Console.WriteLine("Melon: Something went wrong. I'm Sorry :(");
                Console.WriteLine("Melon: Maybe the service is already running?");
            }
        }
        public static void stopService(string serviceName)
        {
            int timeoutMilliseconds = 15000;
            ServiceController service = new ServiceController(serviceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                Console.WriteLine("Melon: Trying to stop " + serviceName + " service. Hold on.");
                service.Stop();
                //Console.WriteLine("Melon: Waiting for " + timeout + " seconds...");
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                Console.WriteLine("Melon: Stopped the Service :)");
            }
            catch
            {
                Console.WriteLine("Melon: Something went wrong. I'm Sorry :(");
                Console.WriteLine("Melon: Maybe the service isn't running.");
            }
        }
        public static void restartService(string serviceName)
        {
            int timeoutMilliseconds = 15000;
            ServiceController service = new ServiceController(serviceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                Console.WriteLine("Melon: Trying to restart " + serviceName + " service. Wait a sec.");
                service.Stop();
                //Console.WriteLine("Melon: Waiting for " + timeout + " seconds...");
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                System.Threading.Thread.Sleep(5000);
                service.Start();
                Console.WriteLine("Melon: Restarting...");
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                Console.WriteLine("Melon: Done. restarted Service :)");
            }
            catch
            {
                Console.WriteLine("Melon: Something went wrong. I'm Sorry :(");
            }
        }
        public static string checkStatusService(string serviceName)
        {
            ServiceController service = new ServiceController(serviceName);
            switch (service.Status)
            {
                case ServiceControllerStatus.Running:
                    return "Running";
                case ServiceControllerStatus.Stopped:
                    return "Stopped";
                case ServiceControllerStatus.Paused:
                    return "Paused";
                case ServiceControllerStatus.StopPending:
                    return "Stopping";
                case ServiceControllerStatus.StartPending:
                    return "Starting";
                default:
                    return "Status Changing";
            }
        }
        #endregion execute
    }
}
