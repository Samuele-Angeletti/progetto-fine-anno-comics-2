using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public static class SaveAndLoadSystem
{
    private static Dictionary<string, object> database = new Dictionary<string, object>();

    public static void StoreSaveData(string key, object infos)
    {
        if (database.ContainsKey(key))
        {
            database[key] = infos;
        }
        else
        {
            database.Add(key, infos);
        }

        Debug.Log($"key: {key} infos:{database[key].ToString()}");
    }


    /// <summary>
    /// Save the file with file name in Application path / SaveData folder
    /// </summary>
    /// <param name="objectToSave"></param>
    /// <param name="fileName">File name (do not include .json in this param)</param>
    public static void Save(object objectToSave, string fileName)
    {
        try
        {
            string jsonString = JsonConvert.SerializeObject(objectToSave);
            File.WriteAllText(Application.dataPath + $"/SaveData/{fileName}.json", jsonString);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// Save the file with file name in Application path / SaveData folder
    /// </summary>
    /// <param name="objectToSave"></param>
    /// <param name="fileName">File name (do not include .json in this param)</param>
    /// <param name="path">The full path including the ending Slash: /</param>
    public static void Save(object objectToSave, string fileName, string path)
    {
        try
        {
            string jsonString = JsonUtility.ToJson(objectToSave);
            File.WriteAllText($"{path}{fileName}.json", jsonString);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// Save the whole DATABASE stored in SaveAndLoadSystem
    /// </summary>
    /// <param name="objectToSave"></param>
    /// <param name="fileName">File name (do not include .json in this param)</param>
    /// <param name="path">The full path including the ending Slash: /</param>
    public static void Save()
    {
        try
        {
            string jsonString = JsonUtility.ToJson(database);
            File.WriteAllText(Application.dataPath + $"/SaveData/SavedDatabase.json", jsonString);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }


    /// <summary>
    /// Load the file in Application path / SaveData folder
    /// </summary>
    /// <param name="fileName">File name (do not include .json in this param)</param>
    /// <returns></returns>
    public static T Load<T>(string fileName)
    {
        try
        {
            string jsonLoaded = File.ReadAllText(Application.dataPath + $"/SaveData/{fileName}.json");
            return JsonUtility.FromJson<T>(jsonLoaded);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return default(T);
        }
    }

    /// <summary>
    /// Load with File Name and full Path.
    /// The full path must include the ending Slash.
    /// </summary>
    /// <param name="fileName">File name (do not include .json in this param)</param>
    /// <param name="path">The full path including the ending Slash: /</param>
    /// <returns></returns>
    public static T Load<T>(string fileName, string path)
    {
        try
        {
            string jsonLoaded = File.ReadAllText($"{path}{fileName}.json");
            return JsonUtility.FromJson<T>(jsonLoaded);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return default(T);
        }
    }
}
