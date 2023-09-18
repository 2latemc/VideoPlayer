using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.IO;
using System.Windows;
using VideoPlayer.MVVM.Model.Utils;

namespace VideoPlayer.MVVM.Model;

public class SaveManager {
    private Settings? _settings;

    private readonly int _maxSizeUntilSave = 1;
    private int _buffer;

    public double? CachedVolumeTilShutdown = null;

    private Dictionary<ulong, TimeSpan>? _videoTimeSpans;

    private Dictionary<ulong, TimeSpan> VideoTimeSpans {
        get {
            if(_videoTimeSpans == null) _videoTimeSpans = new Dictionary<ulong, TimeSpan>();
            return _videoTimeSpans;
        }
        set => _videoTimeSpans = value;
    }

    public TimeSpan GetVideoTimeSpanByFileId(string path) {
        if (!File.Exists(path)) return TimeSpan.Zero;
        ulong fileId = UniqueFileId.GetUniqueIdByFilePath(path);

        if (VideoTimeSpans.TryGetValue(fileId, out var id)) return id;

        return TimeSpan.Zero;
    }

    public void AddVideoTimeSpan(string path, TimeSpan timeSpan) {
        if (!File.Exists(path)) return;
        
        LoadTimeSpans();
        
        ulong fileId = UniqueFileId.GetUniqueIdByFilePath(path);

        VideoTimeSpans[fileId] = timeSpan;
        
        Serializor.ToFile(VideoTimeSpans, StaticVariables.TimeSpansSavePath);
    }

    public void LoadTimeSpans() {
        var timespans = Serializor.FromFile<Dictionary<ulong, TimeSpan>>(StaticVariables.TimeSpansSavePath);
        if (timespans == null) {
            VideoTimeSpans = new Dictionary<ulong, TimeSpan>();
            return;
        }

        VideoTimeSpans = timespans;
    }

    public Settings Settings {
        get {
            if (_settings == null) Load();
            return _settings ?? throw new InvalidOperationException();
        }
        set {
            _settings = value;
            _buffer++;
            if (_buffer >= _maxSizeUntilSave) {
                Save();
                _buffer = 0;
            }
        }
    }

    public void Load() {
        var s = Serializor.FromFile<Settings>(StaticVariables.SettingsSavePath);
        if (s == null) {
            s = new Settings(skipTime: 10, startVolume: 1f);
        }

        Settings = s;
    }

    public void Save() {
        var cached = new Settings(skipTime: Settings.SkipTime, CachedVolumeTilShutdown ?? Settings.StartVolume);
        Serializor.ToFile(cached, StaticVariables.SettingsSavePath);
    }
}