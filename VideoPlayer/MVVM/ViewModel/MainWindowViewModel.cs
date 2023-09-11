using System;
using System.Diagnostics;
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
    
    
    public SaveManager SaveManager;


    #region RelayCommands

    public ICommand PauseCommand { get; set; }
    public ICommand SkipCommand { get; set; }
    public ICommand FocusCommand { get; set; }
    public ICommand TopmostCommand { get; set; }

    public ICommand TimelineSliderValueChanged { get; set; }

    #endregion

    public MainWindowViewModel(Uri currentMediaSource, MediaElement mediaElement) {
        CurrentMediaSource = currentMediaSource;
        _mediaElement = mediaElement;
        SaveManager = new SaveManager();
        SaveManager.Load();
        
        Volume = SaveManager.Settings.StartVolume;

        _mainWindow = (MainWindow)(Application.Current.MainWindow ?? throw new InvalidOperationException());
        
        _mainWindow.VolumeBorder.MouseEnter += (sender, args) => IsMouseOverVolumeHud = true;
        _mainWindow.VolumeBorder.MouseLeave += (sender, args) => IsMouseOverVolumeHud = false;
        
        _mainWindow.TimestampBorder.MouseEnter += (sender, args) => IsMouseOverTimelineHud = true;
        _mainWindow.TimestampBorder.MouseLeave += (sender, args) => IsMouseOverTimelineHud = false;
        
        SetupVideoPlayer();

        PauseCommand = new RelayCommand(PauseVideo);
        SkipCommand = new RelayCommand(SkipVideo);
        FocusCommand = new RelayCommand(o => IsMaximized = !IsMaximized);
        TopmostCommand = new RelayCommand(o => Topmost = !Topmost);
        TimelineSliderValueChanged = new RelayCommand(newValue => TimeSliderValueChanged(newValue));
        SetupEventTick();
    }

    private void TimeSliderValueChanged(object? o) {
        Debug.WriteLine("CHangedlol");
        
        if (!_mediaElement.NaturalDuration.HasTimeSpan)
            return;

        double maxTime = _mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
    
        // Ensure the input value is between 0 and 10
        double value = Math.Min(10.0, Math.Max(0.0, _mainWindow.TimespanSlider.Value));
    
        // Calculate the position to set based on the input value
        double newPosition = (value / 10) * maxTime;
    
        // Set the position of the MediaElement
        _mediaElement.Position = TimeSpan.FromSeconds(newPosition);
        
        OnPropertyChanged(nameof(NormalizedVideoTimeSpan));
    }

    private DispatcherTimer _timer = null!;

    public double NormalizedVideoTimeSpan {
        get {
            if (!_mediaElement.NaturalDuration.HasTimeSpan) return 0;

            TimeSpan maxTime = _mediaElement.NaturalDuration.TimeSpan;
            TimeSpan currentTime = _mediaElement.Position;

            // Convert to seconds for normalization
            double maxSeconds = maxTime.TotalSeconds;
            double currentSeconds = currentTime.TotalSeconds;

            // Calculate the normalized value
            double normalizedValue = currentSeconds / maxSeconds;

            // Ensure the result is between 0 and 1
            normalizedValue = Math.Min(1.0, Math.Max(0.0, normalizedValue));

            return normalizedValue * 10;
        }
    }

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

    public Uri CurrentMediaSource { get; set; }

    private bool _isMaximized;

    private bool _topmost = true;

    public bool Topmost {
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


    #region Visibility

    //volume hud
    private bool _isMouseOverVolumeHud;

    private bool IsMouseOverVolumeHud {
        get => _isMouseOverVolumeHud;
        set {
            _isMouseOverVolumeHud = value;
            OnPropertyChanged(nameof(VolumeHudVisibility));
        }
    }

    public Visibility VolumeHudVisibility {
        get {
            if (IsMouseOverVolumeHud) return Visibility.Visible;
            return Visibility.Hidden;
        }
    }

    //timeline
    private bool _isMouseOverTimelineHud;

    private bool IsMouseOverTimelineHud {
        get => _isMouseOverTimelineHud;
        set {
            _isMouseOverTimelineHud = value;
            OnPropertyChanged(nameof(TimelineHudVisibility));
        }
    }

    public Visibility TimelineHudVisibility {
        get {
            if (IsMouseOverTimelineHud) return Visibility.Visible;
            return Visibility.Hidden;
        }
    }
    
    
    #endregion

    public double Volume {
        get => _mediaElement.Volume;
        set {
            _mediaElement.Volume = value;
            SaveManager.CachedVolumeTilShutdown = value;
            OnPropertyChanged(nameof(VolumeText));
            OnPropertyChanged();
        }
    }

    public string VolumeText {
        get {
            int volume = (int)(Volume * 100);
            if (volume == 69) volume = 68;
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
            float skipTime = SaveManager.Settings.SkipTime;
            if (isLeft) skipTime = -skipTime; /* IsLeft*/

            _mediaElement.Position += TimeSpan.FromSeconds(skipTime);
        }
        catch {
            throw new ArgumentException("Invalid input");
        }
    }
}