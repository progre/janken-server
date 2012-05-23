using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace janken_client
{
    class Program
    {
        static void Main(string[] args)
        {
            int seed;
            string target;
            Console.WriteLine("hello TEST_PROGRAM");
            for (; ; )
            {
                try
                {
                    var init = Console.ReadLine().Split(' ');
                    if (init[0] != "init")
                    {
                        continue;
                    }
                    seed = int.Parse(init[1]);
                    target = init[2];
                    break;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            var rnd = new Random(seed);
            for (; ; )
            {
                for (; ; )
                {
                    try
                    {
                        var janken = Console.ReadLine().Split(' ');
                        if (janken[0] == "end")
                        {
                            return;
                        }
                        if (janken[0] != "janken")
                        {
                            continue;
                        }
                        Console.WriteLine("pon " + (rnd.Next(3) + 1) + " じゃーんけーん、死ねえ！");
                        break;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                for (; ; )
                {
                    try
                    {
                        var init = Console.ReadLine().Split(' ');
                        if (init[0] != "pon")
                        {
                            continue;
                        }
                        break;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }
    }
}
