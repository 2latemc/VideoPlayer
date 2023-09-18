using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace VideoPlayer {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            MouseRightButtonDown += RightMouseDown;
            MouseRightButtonUp += RightMouseButtonUp;
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

        
        public event EventHandler? SliderDragCompletedEvent;
        private void TimespanSlider_OnValueChanged(object sender, DragCompletedEventArgs dragCompletedEventArgs) {
            SliderDragCompletedEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}