using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using ТПП_ЛР9;

namespace ТПП_ЛР9
{
    public partial class PersonalDataPage : Page
    {
        private MainWindow _mainWindow;

        public PersonalDataPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            if (_mainWindow.FormData != null)
            {
                FirstNameTextBox.Text = _mainWindow.FormData.FirstName;
                LastNameTextBox.Text = _mainWindow.FormData.LastName;
                BirthDatePicker.SelectedDate = _mainWindow.FormData.BirthDate;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите имя");
                return;
            }

            if (string.IsNullOrWhiteSpace(LastNameTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите фамилию");
                return;
            }

            if (BirthDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите дату рождения");
                return;
            }

            _mainWindow.FormData.FirstName = FirstNameTextBox.Text;
            _mainWindow.FormData.LastName = LastNameTextBox.Text;
            _mainWindow.FormData.BirthDate = BirthDatePicker.SelectedDate;

            _mainWindow.MainFrame.Navigate(new ContactDataPage(_mainWindow));
        }
    }
}