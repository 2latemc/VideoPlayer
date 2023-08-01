using System;

namespace VideoPlayer.Code.Utils; 

public class UrlUtils {
    public static bool IsValidFileUrl(string fileUrl) {
        bool success = Uri.TryCreate(fileUrl, UriKind.Absolute, out Uri? uriResult);

        if (success && uriResult != null)
            return uriResult.Scheme == Uri.UriSchemeFile;
        return false;
    }
}