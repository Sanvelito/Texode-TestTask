using ScottPlot;
using Steps.Commands;
using Steps.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Steps.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;
        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }
        public ICommand UpdateViewCommand { get; set; }

        public MainViewModel()
        {
            
            this.PropertyChanged += SelectedUserChanged;
            allUsers = AccountData.GetActiveUsers();
            daysteps = AccountData.GetAllDaySteps(SelectedUser);

            double[] dataX = new double[] { 1 };
            double[] dataY = new double[] { 1 };
            MainWindow.WpfPlot.Plot.AddScatter(dataX, dataY);
            MainWindow.WpfPlot.Refresh();
        }

        private List<Account> allUsers;
        public List<Account> AllUsers
        {
            get
            {
                return allUsers;
            }
            set
            {
                allUsers = value;
                OnPropertyChanged("allUsers");
            }
        }

        #region USERS
        private string user;
        public string User
        {
            get => user;
            set
            {
                user = value;
                OnPropertyChanged("User");
            }
        }

        private double average;
        public double Average
        {
            get => average;
            set
            {
                average = value;
                OnPropertyChanged("Average");
            }
        }
        private double maximum;
        public double Maximum
        {
            get => maximum;
            set
            {
                maximum = value;
                OnPropertyChanged("Maximum");
            }
        }
        private double minimum;
        public double Minimum
        {
            get => minimum;
            set
            {
                minimum = value;
                OnPropertyChanged("Minimum");
            }
        }
        private List<int> daysteps;
        public List<int> Daysteps
        {
            get => daysteps;
            set
            {
                daysteps = value;
                OnPropertyChanged("Daysteps");
            }
        }
        private string underrate;
        public string Underrate
        {
            get => underrate;
            set
            {
                underrate = value;
                OnPropertyChanged("Underrate");
            }
        }

        #endregion
        private void UpdateAllUsersView()
        {
            AllUsers = AccountData.GetActiveUsers();
            MainWindow.AllUsersView.ItemsSource = null;
            MainWindow.AllUsersView.Items.Clear();
            MainWindow.AllUsersView.ItemsSource = AllUsers;
            MainWindow.AllUsersView.Items.Refresh();
        }
        private Account selectedUser;
        public Account SelectedUser
        {
            get => selectedUser;
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        public EventHandler ContainerStatusChanged { get; private set; }

        private void SelectedUserChanged(object sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(SelectedUser))
            {
                if (SelectedUser != null)
                {
                    Daysteps = AccountData.GetAllDaySteps(SelectedUser);
                    MainWindow.WpfPlot.Plot.Clear();
                    double[] dataX = new double[Daysteps.Count];
                    double[] dataY = new double[Daysteps.Count];
                    double[] dataX2 = new double[1];
                    double[] dataY2 = new double[1] { Convert.ToDouble(SelectedUser.Maximum) };
                    double[] dataX3 = new double[1];
                    double[] dataY3 = new double[1] { Convert.ToDouble(SelectedUser.Minimum) };

                    for (int i = 0; i < Daysteps.Count; i++)
                    {
                        dataX[i] = i;
                        dataY[i] = Convert.ToDouble(Daysteps[i]);
                        if (dataY[i] == dataY2[0])
                        {
                            dataX2[0] = dataX[i];
                        }
                        if (dataY[i] == dataY3[0])
                        {
                            dataX3[0] = dataX[i];
                        }

                    }
                    MainWindow.WpfPlot.Plot.AddScatter(dataX, dataY);
                    MainWindow.WpfPlot.Plot.AddMarker(x: dataX2[0], y: dataY2[0], color: System.Drawing.Color.Green).Text = "Max";
                    MainWindow.WpfPlot.Plot.AddMarker(x: dataX3[0], y: dataY3[0], color: System.Drawing.Color.Red).Text = "Min";
                    MainWindow.WpfPlot.Refresh();
                }
                else
                {

                }
            }
        }

        private DelegateCommand exportJson;
        public DelegateCommand ExportJson
        {
            get
            {
                return exportJson ?? new DelegateCommand(obj =>
                {
                    if (SelectedUser != null)
                    {
                        AccountData.SaveUserStats(SelectedUser);
                    }
                }
          );
            }
        }
    }
}
