using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace VideoPlayer.MVVM.Model.Utils;

public class UniqueFileId {
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool GetFileInformationByHandle(IntPtr hFile, out ByHandleFileInformation lpFileInformation);

    public struct ByHandleFileInformation {
        public uint FileAttributes;
        public FILETIME CreationTime;
        public FILETIME LastAccessTime;
        public FILETIME LastWriteTime;
        public uint VolumeSerialNumber;
        public uint FileSizeHigh;
        public uint FileSizeLow;
        public uint NumberOfLinks;
        public uint FileIndexHigh;
        public uint FileIndexLow;
    }

    public static ulong GetUniqueIdByFilePath(string path) {
        if (!File.Exists(path)) {
            throw new FileNotFoundException("Error while getting fileID of path " + path + ". The given File does not exist.");
        }


        ByHandleFileInformation objectFileInfo;

        FileInfo fi = new FileInfo(path);
        using FileStream fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        if(fs.SafeFileHandle.IsInvalid) return default;
        
        GetFileInformationByHandle(fs.SafeFileHandle.DangerousGetHandle(), out objectFileInfo);
        
        ulong fileIndex = ((ulong)objectFileInfo.FileIndexHigh << 32) + objectFileInfo.FileIndexLow;

        return fileIndex;
    }
}