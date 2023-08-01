using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualBasic.CompilerServices;
using VideoPlayer.MVVM.Model.Utils;

namespace VideoPlayer.MVVM.ViewModel;

public class MainWindowViewModel : ViewModelBase {
    private MediaElement _mediaElement;
    private MainWindow _mainWindow;
    
    public MainWindowViewModel(Uri currentMediaSource, MediaElement mediaElement) {
        CurrentMediaSource = currentMediaSource;
        _mediaElement = mediaElement;
        _mediaElement.Volume = 1;
        
        _mainWindow = (MainWindow)Application.Current.MainWindow;
        _mainWindow.VolumeBorder.MouseEnter += (sender, args) => IsMouseOverHud = true;
        _mainWindow.VolumeBorder.MouseLeave += (sender, args) => IsMouseOverHud = false;
            SetupVideoPlayer();
    }
    
    public Uri CurrentMediaSource { get; set; }

    private bool _isMouseOverHud;
    private bool IsMouseOverHud {
        get => _isMouseOverHud;
        set {
            _isMouseOverHud = value;
            OnPropertyChanged(nameof(HudVisibility));
        }
    }

    public Visibility HudVisibility {
        get {
            if (IsMouseOverHud) return Visibility.Visible;
            return Visibility.Hidden;
        }
    }

    public double Volume {
        get => _mediaElement.Volume;
        set {
            _mediaElement.Volume = value;
            OnPropertyChanged(nameof(VolumeText));
            OnPropertyChanged();
        }
    }

    public string VolumeText {
        get {
            int volume = (int)(Volume * 100);
            switch (volume) {
                case 0:
                    return "Muted";
                default:
                    return volume.ToString();
            }
        }
    }

    private void SetupVideoPlayer() {
        try {
            _mediaElement.Play();
        }
        catch (Exception ex) {
            ErrorUtils.ShowError("Could not play video", ex.Message);
        }
    }
}