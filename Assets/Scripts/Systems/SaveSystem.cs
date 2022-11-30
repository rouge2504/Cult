using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem 
{
    private static SaveSystem instance = null;

    private static readonly string URL_EDITOR = Application.streamingAssetsPath;
    private static readonly string URL_ANDROID = Application.persistentDataPath;


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

    public void Write(object o, string jsonPath)
    {
        string path = URL_EDITOR + "/" + jsonPath;
        string json = JsonUtility.ToJson(o);
        File.WriteAllText(path, json);
    }
}
