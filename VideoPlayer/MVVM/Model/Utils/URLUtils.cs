using System;

namespace VideoPlayer.MVVM.Model.Utils; 

public class UrlUtils {
    public static bool IsValidFileUrl(string fileUrl) {
        bool success = Uri.TryCreate(fileUrl, UriKind.Absolute, out Uri? uriResult);

        if (success && uriResult != null)
            return uriResult.Scheme == Uri.UriSchemeFile;
        return false;
    }
}