using System;
using System.Runtime.Serialization;

namespace VideoPlayer.MVVM.Model; 

[DataContract]
public struct Settings {
    [DataMember] public float SkipTime = 10;
    
    [DataMember] public double StartVolume = 1f;

    public Settings(float skipTime, double startVolume) {
        SkipTime = skipTime;
        StartVolume = startVolume;
    }
}