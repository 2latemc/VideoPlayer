using System;
using System.Runtime.Serialization;

namespace VideoPlayer.MVVM.Model; 

public class Settings {
    [DataMember] public float SkipTime = 10;
    
    [DataMember] public float StartVolume = 1f;
}