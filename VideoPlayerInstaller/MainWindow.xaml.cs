using System.Windows;
using System.Windows.Input;

namespace VideoPlayerInstaller {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e) {
            DragMove();
        }
    }
}