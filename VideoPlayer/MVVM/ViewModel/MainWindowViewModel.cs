using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using VideoPlayer.MVVM.Model;
using VideoPlayer.MVVM.Model.Utils;

namespace VideoPlayer.MVVM.ViewModel;

public class MainWindowViewModel : ViewModelBase {
    private MediaElement _mediaElement;
    private MainWindow _mainWindow;
    private SaveManager _saveManager;

    public MainWindowViewModel(Uri currentMediaSource, MediaElement mediaElement) {
        CurrentMediaSource = currentMediaSource;
        _mediaElement = mediaElement;
        _saveManager = new SaveManager();
        _mediaElement.Volume = _saveManager.Settings.StartVolume;


        _mainWindow = (MainWindow)(Application.Current.MainWindow ?? throw new InvalidOperationException());
        _mainWindow.VolumeBorder.MouseEnter += (sender, args) => IsMouseOverHud = true;
        _mainWindow.VolumeBorder.MouseLeave += (sender, args) => IsMouseOverHud = false;
        SetupVideoPlayer();

        PauseCommand = new RelayCommand(PauseVideo);
        SkipCommand = new RelayCommand(SkipVideo);
        FocusCommand = new RelayCommand((o => IsMaximized = !IsMaximized));
        TopmostCommand = new RelayCommand(o => Topmost = !Topmost);

        SetupEventTick();
    }

    private DispatcherTimer _timer;

    private void SetupEventTick() {
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(100);
        _timer.Tick += Tick;
        _timer.Start();
    }

    private void Tick(object? sender, EventArgs e) {
        OnPropertyChanged(nameof(VideoTime));
    }

    public string VideoTime {
        get {
            var pos = _mediaElement.Position;
            return pos.Hours > 0
                ? $"{pos.Hours:D2}:{pos.Minutes:D2}"
                : $"{pos.Minutes}:{pos.Seconds:D2}";
        }
    }

    public float MediaElementSliderPos {
        set {
            double totalDurationInSeconds = _mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
            TimeSpan newPosition = TimeSpan.FromSeconds(value * totalDurationInSeconds);
            _mediaElement.Position = newPosition;
        }
    }

    public Uri CurrentMediaSource { get; set; }

    private bool _isMaximized;

    private bool _topmost = true;

    private bool Topmost {
        get => _topmost;
        set {
            _topmost = value;
            OnPropertyChanged();
        }
    }

    private bool IsMaximized {
        get => _isMaximized;
        set {
            _isMaximized = value;
            _mainWindow.WindowState = WindowState;
        }
    }

    public WindowState WindowState {
        get {
            if (_isMaximized) return WindowState.Maximized;
            return WindowState.Normal;
        }
    }

    /* Relay Commands */

    public ICommand PauseCommand { get; set; }
    public ICommand SkipCommand { get; set; }
    public ICommand FocusCommand { get; set; }

    public ICommand TopmostCommand { get; set; }

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


    private bool _videoPaused;

    private void PauseVideo(object o) {
        if (!_mediaElement.CanPause) {
            // TODO: Error handling
            ErrorUtils.ShowError("Could not pause idk why lol");
            return;
        }

        if (_videoPaused) {
            _mediaElement.Play();
        }
        else {
            _mediaElement.Pause();
        }

        _videoPaused = !_videoPaused;
    }

    private void SkipVideo(object o) {
        try {
            bool isLeft = Convert.ToBoolean(o);
            Console.WriteLine();
            float skipTime = _saveManager.Settings.SkipTime;
            if (isLeft) skipTime = -skipTime; /* IsLeft*/

            _mediaElement.Position += TimeSpan.FromSeconds(skipTime);
        }
        catch {
            throw new ArgumentException("Invalid input");
        }
    }
}