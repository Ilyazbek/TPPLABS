using System;
using System.ComponentModel;
using System.Windows.Input;

namespace ТПП_ЛР4
{
    public class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => canExecute == null || canExecute();

        public void Execute(object parameter) => execute();

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }

    public class GameViewModel : INotifyPropertyChanged
    {
        private int currentValue;
        private int attempts = 1;
        private string resultMessage;
        private string attemptMessage;
        private string userInput;
        private string buttonContent = "Отправить";

        public string ResultMessage
        {
            get { return resultMessage; }
            set
            {
                resultMessage = value; OnPropertyChanged(nameof(ResultMessage));
            }
        }
        public string AttemptMessage
        {
            get { return attemptMessage; }
            set
            {
                attemptMessage = value; OnPropertyChanged(nameof(AttemptMessage));
            }
        }
        public string UserInput
        {
            get { return userInput; }
            set
            {
                userInput = value; OnPropertyChanged(nameof(UserInput));
            }
        }

        public string ButtonContent
        {
            get { return buttonContent; }
            set
            {
                buttonContent = value; OnPropertyChanged(nameof(ButtonContent));
            }
        }

        public GameViewModel()
        {
            ResetGame();
        }

        public void ResetGame()
        {
            Random random = new Random();
            currentValue = random.Next(1, 100);
            attempts = 1;
            ButtonContent = "Отправить";
            UserInput = "";
            ResultMessage = "";
            AttemptMessage = "Попытка №" + attempts.ToString();
            Command = new RelayCommand(UserGuess);
        }

        private ICommand _command;
        public ICommand Command
        {
            get
            {
                return _command;
            }
            set
            {
                _command = value;
                OnPropertyChanged(nameof(Command));
            }
        }

        public void UserGuess()
        {
            int guess;
            if (!int.TryParse(userInput, out guess))
            {
                ResultMessage = "Неправильно введено значение!";
            }
            else if (guess < 1 || guess > 100)
            {
                ResultMessage = "Число должно быть от 1 до 100!";
            }
            else if (guess == currentValue)
            {
                ResultMessage = "Поздравляем, вы угадали!";
                Command = new RelayCommand(ResetGame);
                ButtonContent = "Перезапустить";
            }
            else if (guess > currentValue)
            {
                ResultMessage = "Загаданное число меньше";
                attempts++;
                AttemptMessage = "Попытка №" + attempts.ToString();
            }
            else if (guess < currentValue)
            {
                ResultMessage = "Загаданное число больше";
                attempts++;
                AttemptMessage = "Попытка №" + attempts.ToString();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
