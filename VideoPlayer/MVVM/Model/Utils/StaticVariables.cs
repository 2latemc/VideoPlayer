using System;
using System.IO;

namespace VideoPlayer.MVVM.Model.Utils; 

public abstract class StaticVariables {
    public static string SettingsSavePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"VideoPlayer\settings.xaml");
    public static string TimeSpansSavePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"VideoPlayer\timespans.xaml");
}