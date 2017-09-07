using Order.Core.Context;
using OrderTestWPF.ViewModel;
using System.Windows;

namespace OrderTestWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowModel(new Repository());
        }
    }
}