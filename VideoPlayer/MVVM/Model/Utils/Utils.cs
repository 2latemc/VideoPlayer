﻿using System.IO;

namespace VideoPlayer.MVVM.Model.Utils;

public class Utils {
    public static bool IsVideoFile(string filePath)
    {
        string extension = Path.GetExtension(filePath);

        if (!string.IsNullOrEmpty(extension))
        {
            string[] videoExtensions = { ".mp4", ".avi", ".mkv", ".mov", ".wmv" }; // Add more video extensions if needed
            foreach (var videoExtension in videoExtensions) {
                if (filePath.EndsWith(videoExtension)) return true;
            }
        }

        return false;
    }
}