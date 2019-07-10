using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicPlayerOnline.ViewModel
{
    class RelayCommand : ICommand
    {
        Func<object, bool> _canExecute;
        Action<Object> _execute;

        public RelayCommand(Action<Object> execute)
            : this(execute, null)
        {
        }
        public RelayCommand(Action<Object> execute, Func<object, bool> canExecute)
        {
            if (execute == null)
            {
                throw new Exception("execute must be initialized");
            }
            _canExecute = canExecute;
            _execute = execute;
        }
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;
            return _canExecute(parameter);
        }
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
