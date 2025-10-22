using System.Windows;
using Microsoft.SqlServer.Server;

namespace ТПП_ЛР9
{
    public partial class MainWindow : Window
    {
        public FormData FormData { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            FormData = new FormData();
            MainFrame.Navigate(new PersonalDataPage(this));
        }
    }
}