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

    public MainWindowViewModel(Uri currentMediaSource, MediaElement mediaElement) {
        ShowHudBorderMoueOverChangedCommand = new RelayCommand(o => {
            try {
                var args = (DependencyPropertyChangedEventArgs)o;
                _isMouseOverHud = (bool)args.NewValue;
                OnPropertyChanged(nameof(HudVisibility));
            }
            catch {
                Debug.WriteLine("Invalid binding fix this please lol");
                return;
            }
        });

        CurrentMediaSource = currentMediaSource;
        _mediaElement = mediaElement;
        _mediaElement.Volume = 1;
        SetupVideoPlayer();
    }


    public RelayCommand ShowHudBorderMoueOverChangedCommand { get; set; }

    public Uri CurrentMediaSource { get; set; }

    private bool _isMouseOverHud;

    public Visibility HudVisibility {
        get {
            if (_isMouseOverHud) return Visibility.Visible;
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