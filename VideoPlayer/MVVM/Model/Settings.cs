using System;
using System.Runtime.Serialization;

namespace VideoPlayer.MVVM.Model; 

[DataContract]
public class Settings {
    [DataMember] public float SkipTime;
    
    [DataMember] public double StartVolume;

    public Settings(float skipTime, double startVolume) {
        SkipTime = skipTime;
        StartVolume = startVolume;
    }
}