using System;
using System.Windows.Input;

namespace VideoPlayerInstaller.MVVM;

public class RelayCommand : ICommand {
    private Func<object?, bool>? _canExecute;
    private Action<object?> _execute;

    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null) {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter) => _canExecute == null || _canExecute.Invoke(parameter);

    public void Execute(object? parameter) => _execute.Invoke(parameter);

    public event EventHandler? CanExecuteChanged {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}