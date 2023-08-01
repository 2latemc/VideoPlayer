using System;
using System.Windows;

namespace VideoPlayer.MVVM.Model.Utils;

public class ErrorUtils {
    public static void ShowError(Error error) {
        ShowMessageBox(error.Title, error.Message);
        if (error.ShutDown) Environment.Exit(0);
    }

    public static void ShowError(string content, bool shutDown = false) =>
        ShowError(new Error("Error", content, shutDown));

    public static void ShowError(string title, string content, bool shutDown = false) =>
        ShowError(new Error(title, content, shutDown));

    public struct Error {
        public string Title;
        public string Message;
        public bool ShutDown;

        public Error(string title, string message, bool shutDown) {
            Title = title;
            Message = message;
            ShutDown = shutDown;
        }
    }

    private static void ShowMessageBox(string title, string message) => MessageBox.Show(title, message);
}