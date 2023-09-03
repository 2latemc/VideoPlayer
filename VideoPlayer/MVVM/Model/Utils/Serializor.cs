using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace VideoPlayer.MVVM.Model.Utils; 

public class Serializor {
    public static void ToFile<T>(T obj, string path) {
        try {
            var serializer = new DataContractSerializer(typeof(T));
            using var writer = new FileStream(path, FileMode.OpenOrCreate);

            serializer.WriteObject(writer, obj);
        }
        catch(Exception e) {
            Debug.WriteLine("There was an error when serializing " + e);
        }
    }

    public static T? FromFile<T>(string path) {
        try {
            if(!File.Exists(path)) return default;
            
            var serializer = new DataContractSerializer(typeof(T));
            using var reader = new FileStream(path, FileMode.Open);

            return (T) serializer.ReadObject(reader)!;
        }
        catch (Exception e) {
            Debug.WriteLine("There was an error when deserializing: " + e);
            return default;
        }
    }
}