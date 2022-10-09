using ScottPlot;
using Steps.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Steps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ListView AllUsersView;
        public static WpfPlot WpfPlot;
        public MainWindow()
        {
            InitializeComponent();

            AllUsersView = ViewAllUsers;
            WpfPlot = WpfPlot1;
            DataContext = new MainViewModel();
           
        }
    }
}
