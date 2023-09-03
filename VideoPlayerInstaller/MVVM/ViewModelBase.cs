using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace VideoPlayerInstaller.MVVM; 

public class ViewModelBase : Window {
    public event PropertyChangedEventHandler? PropChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        PropChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}