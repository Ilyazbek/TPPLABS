using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Linq;

namespace ТПП_ЛР9
{
    public partial class ContactDataPage : Page
    {
        private MainWindow _mainWindow;

        public ContactDataPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            EmailTextBox.Text = _mainWindow.FormData.Email;
            PhoneTextBox.Text = _mainWindow.FormData.Phone;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.FormData.Email = EmailTextBox.Text;
            _mainWindow.FormData.Phone = PhoneTextBox.Text;

            _mainWindow.MainFrame.Navigate(new PersonalDataPage(_mainWindow));
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Пожалуйста, введите корректный email");
                return;
            }

            if (string.IsNullOrWhiteSpace(PhoneTextBox.Text) || !PhoneTextBox.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Пожалуйста, введите корректный телефонный номер");
                return;
            }

            _mainWindow.FormData.Email = EmailTextBox.Text;
            _mainWindow.FormData.Phone = PhoneTextBox.Text;

            _mainWindow.MainFrame.Navigate(new AddressPage(_mainWindow));
        }
    }
}