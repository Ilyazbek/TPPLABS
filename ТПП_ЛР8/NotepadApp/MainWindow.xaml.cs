using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;
using NotepadApp.Properties;
using System.Threading;
using System.Windows.Controls;

namespace NotepadApp
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _currentFilePath;
        private string _lastSavedText = string.Empty;
        private CultureInfo _currentCulture = new CultureInfo("en");

        public event PropertyChangedEventHandler PropertyChanged;

        #region Bindable Properties

        public string WindowTitle
        {
            get
            {
                var baseTitle = Strings.WindowTitle;
                if (string.IsNullOrEmpty(_currentFilePath))
                {
                    return HasUnsavedChanges() ? $"{Strings.NewFileTitle}*" : Strings.NewFileTitle;
                }
                else
                {
                    var fileName = Path.GetFileName(_currentFilePath);
                    return HasUnsavedChanges() ? $"{Strings.WindowTitle} - {fileName}*" : $"{Strings.WindowTitle} - {fileName}";
                }
            }
        }

        public string MenuFile => Strings.MenuFile;
        public string MenuNew => Strings.MenuNew;
        public string MenuOpen => Strings.MenuOpen;
        public string MenuSave => Strings.MenuSave;
        public string MenuExit => Strings.MenuExit;
        public string MenuLanguage => Strings.MenuLanguage;
        public string MenuEnglish => Strings.MenuEnglish;
        public string MenuRussian => Strings.MenuRussian;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            TextEditor.TextChanged += TextEditor_TextChanged;
            LoadLanguageSettings();
        }

        private void LoadLanguageSettings()
        {

            ChangeLanguage("ru");
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateAllProperties()
        {
            OnPropertyChanged(nameof(WindowTitle));
            OnPropertyChanged(nameof(MenuFile));
            OnPropertyChanged(nameof(MenuNew));
            OnPropertyChanged(nameof(MenuOpen));
            OnPropertyChanged(nameof(MenuSave));
            OnPropertyChanged(nameof(MenuExit));
            OnPropertyChanged(nameof(MenuLanguage));
            OnPropertyChanged(nameof(MenuEnglish));
            OnPropertyChanged(nameof(MenuRussian));
        }

        private void TextEditor_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            OnPropertyChanged(nameof(WindowTitle));
        }

        private void LanguageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.Tag is string cultureCode)
            {
                ChangeLanguage(cultureCode);
            }
        }

        private void ChangeLanguage(string cultureCode)
        {
            _currentCulture = new CultureInfo(cultureCode);
            Thread.CurrentThread.CurrentUICulture = _currentCulture;
            Thread.CurrentThread.CurrentCulture = _currentCulture;

            CultureInfo.DefaultThreadCurrentCulture = _currentCulture;
            CultureInfo.DefaultThreadCurrentUICulture = _currentCulture;

            UpdateAllProperties();
        }

        private bool HasUnsavedChanges()
        {
            return _lastSavedText != TextEditor.Text;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (HasUnsavedChanges())
            {
                var result = MessageBox.Show(Strings.UnsavedChangesMessage,
                                           Strings.WindowTitle,
                                           MessageBoxButton.YesNoCancel,
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    SaveCommand_Executed(null, null);

                    if (HasUnsavedChanges())
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }

            base.OnClosing(e);
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (HasUnsavedChanges())
            {
                var result = MessageBox.Show(Strings.UnsavedChangesMessage,
                                           Strings.WindowTitle,
                                           MessageBoxButton.YesNoCancel,
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    SaveCommand_Executed(null, null);
                    if (HasUnsavedChanges()) return;
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            TextEditor.Clear();
            _currentFilePath = null;
            _lastSavedText = string.Empty;
            OnPropertyChanged(nameof(WindowTitle));
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (HasUnsavedChanges())
            {
                var result = MessageBox.Show(Strings.UnsavedChangesMessage,
                                           Strings.WindowTitle,
                                           MessageBoxButton.YesNoCancel,
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    SaveCommand_Executed(null, null);
                    if (HasUnsavedChanges()) return;
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            var openFileDialog = new OpenFileDialog
            {
                Filter = Strings.FileFilter,
                Title = Strings.OpenFileDialogTitle,
                DefaultExt = ".txt"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    TextEditor.Text = File.ReadAllText(openFileDialog.FileName);
                    _currentFilePath = openFileDialog.FileName;
                    _lastSavedText = TextEditor.Text;
                    OnPropertyChanged(nameof(WindowTitle));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{Strings.OpenError}\n{ex.Message}",
                                  Strings.WindowTitle,
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
            }
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentFilePath))
            {
                SaveAs();
            }
            else
            {
                SaveToFile(_currentFilePath);
            }
        }

        private void SaveAs()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = Strings.FileFilter,
                Title = Strings.SaveFileDialogTitle,
                DefaultExt = ".txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                _currentFilePath = saveFileDialog.FileName;
                SaveToFile(_currentFilePath);
            }
        }

        private void SaveToFile(string filePath)
        {
            try
            {
                File.WriteAllText(filePath, TextEditor.Text);
                _lastSavedText = TextEditor.Text;
                OnPropertyChanged(nameof(WindowTitle));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Strings.SaveError}\n{ex.Message}",
                               Strings.WindowTitle,
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
            }
        }

        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}