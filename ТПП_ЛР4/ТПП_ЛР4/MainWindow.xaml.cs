using System;
using System.Windows;

namespace ТПП_ЛР4
{
    public partial class MainWindow : Window
    {
        private GameViewModel viewModel = new GameViewModel();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
