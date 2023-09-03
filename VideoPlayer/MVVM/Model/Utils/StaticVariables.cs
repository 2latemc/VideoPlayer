using System;
using System.IO;

namespace VideoPlayer.MVVM.Model.Utils; 

public abstract class StaticVariables {
    public static string SavePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "settings.xaml");
}