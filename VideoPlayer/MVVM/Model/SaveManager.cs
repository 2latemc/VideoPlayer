﻿using System;
using System.Diagnostics;
using System.Windows;
using VideoPlayer.MVVM.Model.Utils;

namespace VideoPlayer.MVVM.Model;

public class SaveManager {
    private Settings? _settings;

    private readonly int MAX_SIZE_UNTIL_SAVE = 1;
    private int _buffer;

    public double? CachedVolumeTilShutdown = null;

    public Settings Settings {
        get {
            if (_settings == null) Load();
            return _settings ?? throw new InvalidOperationException();
        }
        set {
            _settings = value;
            _buffer++;
            if (_buffer >= MAX_SIZE_UNTIL_SAVE) {
                Save();
                _buffer = 0;
            }
        }
    }

    public void Load() {
        _settings = Serializor.FromFile<Settings>(StaticVariables.SavePath);
        if (_settings == null) {
            _settings = new Settings();
            Save();
        }
    }

    public void Save() {
        var cached = new Settings {
            SkipTime = Settings.SkipTime,
            StartVolume = CachedVolumeTilShutdown ?? Settings.StartVolume
        };
        Serializor.ToFile(cached, StaticVariables.SavePath);
    }
}