using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace Progressive.JankenServer.Models
{
    public class Server
    {
        int seed;
        Process client0 = new Process();
        Process client1 = new Process();

        public string Client0Name { get; private set; }
        public string Client1Name { get; private set; }

        public Server(int seed)
        {
            this.seed = seed;
        }

        public bool StartClient()
        {
            InitProcess(client0, Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\client1");
            InitProcess(client1, Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\client2");
            Parallel.Invoke(
                    () => client0.Start(),
                    () => client1.Start());

            Task.Factory.StartNew(() =>
            {
                for (; ; )
                {
                    if (ReceiveHelloCommand(0, ReadLine(0)))
                    {
                        break;
                    }
                }
            }).Wait(60 * 1000);
            Task.Factory.StartNew(() =>
            {
                for (; ; )
                {
                    if (ReceiveHelloCommand(1, ReadLine(1)))
                    {
                        break;
                    }
                }
            }).Wait(60 * 1000);

            if (string.IsNullOrEmpty(Client0Name)
                    || string.IsNullOrEmpty(Client1Name))
            {
                return false;
            }

            WriteLine(0, "init " + seed + " " + Client1Name);
            WriteLine(1, "init " + seed + " " + Client0Name);
            return true;
        }

        public ResultSet Janken()
        {
            WriteLine(0, "janken");
            WriteLine(1, "janken");

            var resultSet = new ResultSet();

            Task.Factory.StartNew(() =>
            {
                for (; ; )
                {
                    var tuple = ReceivePonCommand(0, ReadLine(0));
                    if (!tuple.Item1)
                        continue;
                    resultSet.Client0Hand = tuple.Item2;
                    resultSet.Client0Comment = tuple.Item3;
                    break;
                }
            }).Wait(60 * 1000);
            Task.Factory.StartNew(() =>
            {
                for (; ; )
                {
                    var tuple = ReceivePonCommand(1, ReadLine(1));
                    if (!tuple.Item1)
                        continue;
                    resultSet.Client1Hand = tuple.Item2;
                    resultSet.Client1Comment = tuple.Item3;
                    break;
                }
            }).Wait(60 * 1000);

            WriteLine(0, "pon " + resultSet.Client1Hand + " " + resultSet.Client1Comment);
            WriteLine(1, "pon " + resultSet.Client0Hand + " " + resultSet.Client0Comment);

            return resultSet;
        }

        public void End()
        {
            WriteLine(0, "end");
            WriteLine(1, "end");
            if (!client0.WaitForExit(60 * 1000))
            {
                client0.Kill();
            }
            if (!client1.WaitForExit(60 * 1000))
            {
                client1.Kill();
            }
        }

        private void InitProcess(Process client, string path)
        {
            var startInfo = client.StartInfo;
            //            startInfo.FileName = path + @"\start.bat";
            startInfo.WorkingDirectory = path;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/c \"" + path + "\\start.bat\"";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
        }

        private bool ReceiveHelloCommand(int client, string command)
        {
            try
            {
                var splited = command.Split(' ');
                if (splited[0] != "hello")
                {
                    return false;
                }
                if (splited.Length < 2)
                {
                    return false;
                }
                string name = OnHello(client, splited[1]);
                switch (client)
                {
                    case 0:
                        Client0Name = name;
                        break;
                    case 1:
                        Client1Name = name;
                        break;
                    default:
                        throw new ApplicationException();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private Tuple<bool, int, string> ReceivePonCommand(int client, string command)
        {
            try
            {
                var splited = command.Split(' ');
                if (splited[0] != "pon")
                {
                    return Tuple.Create(false, 0, "");
                }
                string comment = "";
                if (splited.Length >= 3)
                {
                    comment = splited[2];
                }
                var tuple = OnPon(client, int.Parse(splited[1]), comment);
                return Tuple.Create(true, tuple.Item1, tuple.Item2);
            }
            catch (Exception)
            {
                return Tuple.Create(false, 0, "");
            }
        }

        private string OnHello(int client, string name)
        {
            return name;
        }

        private Tuple<int, string> OnPon(int client, int hand, string comment)
        {
            return Tuple.Create(hand, comment);
        }

        private string ReadLine(int client)
        {
            string line;
            switch (client)
            {
                case 0:
                    line = client0.StandardOutput.ReadLine();
                    break;
                case 1:
                    line = client1.StandardOutput.ReadLine();
                    break;
                default:
                    throw new ApplicationException();
            }
            Console.WriteLine("  <--client" + client + ": " + line);
            return line;
        }

        private void WriteLine(int client, string line)
        {
            Console.WriteLine("-->client" + client + ": " + line);
            switch (client)
            {
                case 0:
                    client0.StandardInput.WriteLine(line);
                    break;
                case 1:
                    client1.StandardInput.WriteLine(line);
                    break;
                default:
                    throw new ApplicationException();
            }
        }
    }
}
