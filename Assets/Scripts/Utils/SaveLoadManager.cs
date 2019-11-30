using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoadManager
{
    public static string Path = string.Format ("{0}/FramePerfect", Application.persistentDataPath);

    public static void SaveData<T> (T data) where T : class
    {
        if (!Directory.Exists (Path))
            Directory.CreateDirectory (Path);

        var path = string.Format ("{0}/{1}.bin", Path, data.GetType ().Name);
        IFormatter formatter = new BinaryFormatter ();
        Stream stream = new FileStream (path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

        formatter.Serialize (stream, data);
        stream.Close ();
    }

    public static T LoadData<T> () where T : class
    {
        if (!Directory.Exists (Path))
            Directory.CreateDirectory (Path);

        T d = null;
        var path = string.Format ("{0}/{1}.bin", Path, typeof (T).Name);
        IFormatter formatter = new BinaryFormatter ();
        Stream stream = new FileStream (path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

        if (stream.Length == 0) return null;

        d = (T) formatter.Deserialize (stream);
        stream.Close ();

        return d;
    }
}