using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Progressive.JankenServer.Commons.ViewModel;
using System.Windows.Input;
using Progressive.JankenServer.Commons.ViewModels;
using Progressive.JankenServer.Models;
using System.Threading.Tasks;
using System.Threading;

namespace Progressive.JankenServer.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private Random random = new Random(48465473);
        private Server server;

        public string Client0Name
        {
            get { return server != null ? server.Client0Name : ""; }
        }

        public string Client1Name
        {
            get { return server != null ? server.Client1Name : ""; }
        }

        private int client0Hand;
        public int Client0Hand
        {
            get { return client0Hand; }
            private set
            {
                client0Hand = value;
                NotifyPropertyChanged("Client0Hand");
            }
        }

        private int client1Hand;
        public int Client1Hand
        {
            get { return client1Hand; }
            private set
            {
                client1Hand = value;
                NotifyPropertyChanged("Client1Hand");
            }
        }

        public int Client0Wins { get; private set; }

        public int Client1Wins { get; private set; }

        public DelegateCommand StartCommand { get; private set; }
        public DelegateCommand JankenCommand { get; private set; }
        public DelegateCommand EndCommand { get; private set; }
        public DelegateCommand ClearCommand { get; private set; }

        public event Action Ready = () => { };

        public MainWindowViewModel()
        {
            StartCommand = new DelegateCommand(
                    parameter =>
                    {
                        server = new Server(random.Next());
                        server.StartClient();
                        NotifyPropertyChanged("Client0Name");
                        NotifyPropertyChanged("Client1Name");
                        StartCommand.NotifyCanExecuteChanged();
                        JankenCommand.NotifyCanExecuteChanged();
                        EndCommand.NotifyCanExecuteChanged();
                    },
                    parameter => server == null
            );
            JankenCommand = new DelegateCommand(
                    parameter =>
                    {
                        var resultSet = server.Janken();
                        Client0Hand = resultSet.Client0Hand;
                        Client1Hand = resultSet.Client1Hand;
                        if ((client1Hand == 1 && client0Hand == 3)
                                || (client1Hand == 2 && client0Hand == 1)
                                || (client1Hand == 3 && client0Hand == 2))
                        {
                            Client0Wins++;
                        }
                        else if ((client0Hand == 1 && client1Hand == 3)
                                || (client0Hand == 2 && client1Hand == 1)
                                || (client0Hand == 3 && client1Hand == 2))
                        {
                            Client1Wins++;
                        }
                        Ready();
                    },
                    parameter => server != null
            );
            EndCommand = new DelegateCommand(
                    parameter =>
                    {
                        server.End();
                        server = null;
                        StartCommand.NotifyCanExecuteChanged();
                        JankenCommand.NotifyCanExecuteChanged();
                        EndCommand.NotifyCanExecuteChanged();
                    },
                    parameter => server != null
            );
            ClearCommand = new DelegateCommand(
                    parameter =>
                    {
                        Client0Wins = 0;
                        Client1Wins = 0;
                    },
                    parameter => true
            );
        }
    }
}
