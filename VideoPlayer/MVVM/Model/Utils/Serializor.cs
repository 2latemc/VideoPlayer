using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Windows;
using System.Xml;

namespace VideoPlayer.MVVM.Model.Utils;

public class Serializor {
    public static void ToFile<T>(T obj, string path) {
        try {
            if (!Directory.Exists(Path.GetDirectoryName(StaticVariables.SettingsSavePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(StaticVariables.SettingsSavePath) ??
                                          throw new InvalidOperationException());

            var serializer = new DataContractSerializer(typeof(T));
            using var writer = XmlWriter.Create(path, new XmlWriterSettings { Indent = true });

            serializer.WriteObject(writer, obj);
        }
        catch (Exception e) {
            Debug.WriteLine("There was an error when serializing " + e);
        }
    }

    public static T? FromFile<T>(string path) {
        try {
            if (!File.Exists(path)) return default;

            var serializer = new DataContractSerializer(typeof(T));
            using var reader = new FileStream(path, FileMode.Open);

            return (T)serializer.ReadObject(reader)!;
        }
        catch (SerializationException e) {
            var result = MessageBox.Show("Could not load save file. Do you want to remove it?",
                "Serialization Exception", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes) {
                try {
                    File.Delete(path);
                    return default;
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Could not remove save file.", MessageBoxButton.OK);
                    Environment.Exit(0);
                    return default;
                }
            }

            Environment.Exit(0);
            return default;
        }
        catch (Exception e) {
            Debug.WriteLine("There was an error when deserializing: " + e);
            return default;
        }
    }
}