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
using System.Windows.Shapes;
using System.ComponentModel;
using Progressive.JankenServer.ViewModels;
using System.Threading;
using System.Threading.Tasks;

namespace Progressive.JankenServer.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();

            InitializeComponent();

            client0Stage.LoadedBehavior = MediaState.Manual;
            client1Stage.LoadedBehavior = MediaState.Manual;
            client0Stage.Stretch = Stretch.Fill;
            client1Stage.Stretch = Stretch.Fill;

            var dataContext = (MainWindowViewModel)DataContext;
            dataContext.PropertyChanged += (sender, e) =>
            {
                switch (e.PropertyName)
                {
                    case "Client0Hand":
                        if (isClient0Sazae.IsChecked == true)
                        {
                            LoadSazaeHand(client0Stage, dataContext.Client0Hand);
                        }
                        else
                        {
                            LoadCureHand(client0Stage, dataContext.Client0Hand);
                        }
                        client0Stage.Pause();
                        break;
                    case "Client1Hand":
                        if (isClient1Sazae.IsChecked == true)
                        {
                            LoadSazaeHand(client1Stage, dataContext.Client1Hand);
                        }
                        else
                        {
                            LoadCureHand(client1Stage, dataContext.Client1Hand);
                        }
                        client1Stage.Pause();
                        break;
                }
            };

            dataContext.Ready += () =>
            {
                client0Stage.Play();
                client1Stage.Play();
            };
            client0Stage.MediaEnded += (sender, e) =>
            {
                client0Wins.GetBindingExpression(Label.ContentProperty).UpdateTarget();
                client1Wins.GetBindingExpression(Label.ContentProperty).UpdateTarget();
                dataContext.JankenCommand.Execute(null);
            };
            client1Stage.MediaEnded += (sender, e) =>
            {
                client0Wins.GetBindingExpression(Label.ContentProperty).UpdateTarget();
                client1Wins.GetBindingExpression(Label.ContentProperty).UpdateTarget();
                dataContext.JankenCommand.Execute(null);
            };
        }

        private void LoadSazaeHand(MediaElement element, int hand)
        {
            switch (hand)
            {
                case 1:
                    element.Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"\Movie\sazae-gu-.avi");
                    break;
                case 2:
                    element.Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"\Movie\sazae-tyoki.avi");
                    break;
                case 3:
                    element.Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"\Movie\sazae-pa-.avi");
                    break;
            }
        }

        private void LoadCureHand(MediaElement element, int hand)
        {
            switch (hand)
            {
                case 1:
                    element.Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"\Movie\cure-gu-.avi");
                    break;
                case 2:
                    element.Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"\Movie\cure-tyoki.avi");
                    break;
                case 3:
                    element.Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"\Movie\cure-pa-.avi");
                    break;
            }
        }
    }
}
