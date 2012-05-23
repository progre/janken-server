using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace janken_client_gui
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("pon " + 1 + " -");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("pon " + 2 + " -");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("pon " + 3 + " -");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            int seed;
            string target;
            Console.WriteLine("hello とんび");
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
        }
    }
}
