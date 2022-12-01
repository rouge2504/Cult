using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem 
{
    private static SaveSystem instance = null;

    public static readonly string URL_EDITOR = Application.streamingAssetsPath;
    public static readonly string URL_ANDROID = Application.persistentDataPath;


    private SaveSystem()
    {
    }

    public static SaveSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SaveSystem();
            }
            return instance;
        }
    }
    public object Read(string jsonPath, Type type)
    {
        string path = URL_EDITOR + "/" + jsonPath;
        if (!File.Exists(path))
        {
            Debug.Log("The File Dont Exist");
            object temp = Activator.CreateInstance(type);
            Write(temp, jsonPath);
            return temp;
        }
        string json = File.ReadAllText(path);
  
        object o = JsonUtility.FromJson(json, type);
        return o;
    }

    public object[] Read(Type type, string jsonPath)
    {
        string path = URL_EDITOR + "/" + jsonPath;
        if (!File.Exists(path))
        {
            Debug.Log("The File Dont Exist");
            object[] temp = new object[] { Activator.CreateInstance(type) };
            Write(temp, jsonPath);
            return temp;
        }
        string json = File.ReadAllText(path);

        object[] o = JsonHelper.FromJson<NPC_Character>(json);
        return o;
    }



    public void Write(object o, string jsonPath)
    {
        string path = URL_EDITOR + "/" + jsonPath;
        string json = JsonUtility.ToJson(o);
        File.WriteAllText(path, json);
    }

    public void Write(object[] o, string jsonPath)
    {
        string path = URL_EDITOR + "/" + jsonPath;
        string json = JsonHelper.ToJson(o);
        File.WriteAllText(path, json);
    }

    public bool Exists(string jsonPath)
    {
        string path = URL_EDITOR + "/" + jsonPath;
        return File.Exists(path);
    }
}


public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static T[] FromJson<T>(string json, Type type)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
