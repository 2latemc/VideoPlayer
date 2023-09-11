using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using VideoPlayer.MVVM.Model.Utils;
using VideoPlayer.MVVM.ViewModel;

namespace VideoPlayer {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            CheckArguments();
        }

        private void CheckArguments() {
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length < 2) {
                FilePicker();
                return;
            }

            args = args.Where((element, index) => index != 0).ToArray();

            string url = String.Join(" ", args);
            url = url.Trim('"');

            CheckFile(url);
        }

        private void CheckFile(string url) {
            if (!UrlUtils.IsValidFileUrl(url)) {
                ErrorUtils.ShowError("Invalid url!");
                return;
            }

            if (!File.Exists(url)) {
                ErrorUtils.ShowError("The specified File does not exists.");
                return;
            }

            if (!Utils.IsVideoFile(url)) {
                ErrorUtils.ShowError("Not a video file.");
            }

            var uri = new Uri(url);
            StartPlayer(uri);
        }


        private MainWindow _window;
        private MainWindowViewModel _model;
        private void StartPlayer(Uri uri) {
            _window = new MainWindow();
            _model = new MainWindowViewModel(uri, _window.MediaElement);
            _window.DataContext = _model;
            _window.Show();
        }

        private void FilePicker() {
            var dialog = new OpenFileDialog() {
                Filter = "Video File|*mp4",
                Title = "Pick a video file"
            };
            var result = dialog.ShowDialog();
            if (result == null || result == false) {
                Environment.Exit(0);
                return;
            }
            
            CheckFile(dialog.FileName);
        }

        private void App_OnExit(object sender, ExitEventArgs e) {
            _model.SaveManager.Save();
        }
    }
}