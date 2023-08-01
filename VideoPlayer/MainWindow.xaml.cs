using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using VideoPlayer.Code.Utils;
using VideoPlayer.MVVM.Model.Utils;

namespace VideoPlayer {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            PreviewKeyDown += MainWindow_KeyDown;
            MouseRightButtonDown += RightMouseDown;
            MouseRightButtonUp += RightMouseButtonUp;
            MouseLeftButtonDown += LeftMouseDown;
            MouseMove += Window_MouseMove;
            SizeChanged += MaintainMediaAspectRatio;

            MediaElement.SizeChanged += FirstTimeSize;
        }

        private void FirstTimeSize(object sender, SizeChangedEventArgs sizeChangedEventArgs) {
            MaintainMediaAspectRatio(null, null);
            MediaElement.SizeChanged -= FirstTimeSize;
        }

        private void MaintainMediaAspectRatio(object sender, SizeChangedEventArgs sizeChangedEventArgs) {
            double mediaHeight = MediaElement.NaturalVideoHeight;
            double mediaWidth = MediaElement.NaturalVideoWidth;

            if (mediaHeight > 0 && mediaWidth > 0) {
                double windowWidth = ActualWidth;
                double windowHeight = ActualHeight;

                double windowAspectRatio = windowWidth / windowHeight;
                double mediaAspectRatio = mediaWidth / mediaHeight;

                if (mediaAspectRatio > windowAspectRatio) {
                    windowHeight = windowWidth / mediaAspectRatio;
                }
                else {
                    windowWidth = windowHeight * mediaAspectRatio;
                }

                Width = windowWidth;
                Height = windowHeight;
            }
        }

        private void RightMouseButtonUp(object sender, MouseButtonEventArgs e) {
            if (_isDragging) {
                _isDragging = false;
                ReleaseMouseCapture();
            }
        }


        private bool _isDragging;
        private Point _clickPosition;

        private void RightMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.RightButton == MouseButtonState.Pressed) {
                _isDragging = true;
                _clickPosition = e.GetPosition(this);
                CaptureMouse();
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e) {
            if (_isDragging) {
                Point currentPosition = e.GetPosition(this);
                double offsetX = currentPosition.X - _clickPosition.X;
                double offsetY = currentPosition.Y - _clickPosition.Y;
                Left += offsetX;
                Top += offsetY;
            }
        }
        private void MainWindow_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Space:
                    PauseVideo();
                    break;
                case Key.Left:
                    Skip(true);
                    break;
                case Key.Right:
                    Skip(false);
                    break;
                case Key.F:
                    if (WindowState == WindowState.Maximized) WindowState = WindowState.Normal;
                    else WindowState = WindowState.Maximized;
                    break;
                case Key.T:
                    Topmost = !Topmost;
                    break;
            }
        }

        private void LeftMouseDown(object sender, MouseButtonEventArgs e) {
            PauseVideo();
        }


        private void Skip(bool isLeft) {
            float skipTime = Settings.SkipTime;
            if (isLeft) skipTime = -skipTime;


            MediaElement.Position += TimeSpan.FromSeconds(skipTime);
        }

        private bool _videoPaused;

        private void PauseVideo() {
            if (!MediaElement.CanPause) {
                ErrorUtils.ShowError("Could not pause idk why lol");
                return;
            }

            if (_videoPaused) {
                MediaElement.Play();
                _videoPaused = false;
            }
            else {
                _videoPaused = true;
                MediaElement.Pause();
            }
        }


        private void VolumeBorder_OnIsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e) {
            throw new NotImplementedException();
        }
    }
}