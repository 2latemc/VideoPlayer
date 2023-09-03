using System;
using VideoPlayer.MVVM.Model.Utils;

namespace VideoPlayer.MVVM.Model;

public class SaveManager {
    private Settings? _settings;

    public Settings Settings {
        get {
            if(_settings == null) Load();
            return _settings ?? throw new InvalidOperationException();
        }
        set {
            _settings = value;
            Save();
        }
    }
    
    private void Load() {
        _settings = Serializor.FromFile<Settings>(StaticVariables.SavePath);
        if (_settings == null) {
            _settings = new Settings();
            Save();
        }
    }

    public SaveManager() {
        Load();
    }

    private void Save() {
        Serializor.ToFile(_settings, StaticVariables.SavePath);
    }
}