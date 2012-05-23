using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Progressive.JankenServer.Models;

namespace Progressive.JankenServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server(0);
            server.StartClient();
            for (int i = 0; i < 10; i++)
            {
                var result = server.Janken();
                if (!string.IsNullOrEmpty(result.Client0Comment))
                    Console.WriteLine(server.Client0Name + "「" + result.Client0Comment + "」");
                if (!string.IsNullOrEmpty(result.Client1Comment))
                    Console.WriteLine(server.Client1Name + "「" + result.Client1Comment + "」");
                Console.WriteLine(server.Client0Name + "さんの手は" + ToHandName(result.Client0Hand) + "でした");
                Console.WriteLine(server.Client1Name + "さんの手は" + ToHandName(result.Client1Hand) + "でした");
            }
            server.End();
        }

        static string ToHandName(int hand)
        {
            switch (hand)
            {
                case 1:
                    return "グー";
                case 2:
                    return "チョキ";
                case 3:
                    return "パー";
                default:
                    return "(無効)";
            }
        }
    }
}
