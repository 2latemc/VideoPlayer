using System;
using System.Windows.Input;

namespace VideoPlayerInstaller.MVVM.ViewModel; 

public class MainWindowViewModel {
    public ICommand StartDownloadCommand { get; set; }

    public MainWindowViewModel() {
        StartDownloadCommand = new RelayCommand(o => StartInstall());
    }

    private void StartInstall() {
        Environment.Exit(0);
    }
}