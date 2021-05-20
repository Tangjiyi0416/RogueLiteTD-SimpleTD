using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
public static class SaveLoadManager
{
    public static void Save<T>(T saveData, string fileName) where T : SaveData
    {
        string json = JsonUtility.ToJson(saveData, true);
        Debug.Log($"Saving as JSON: {json}");
        File.WriteAllText($"{Application.persistentDataPath}/{fileName}", json);
    }
    public static T Load<T>(string fileName) where T : SaveData
    {
        string json;
        try
        {
            json = File.ReadAllText($"{Application.persistentDataPath}/{fileName}");
            Debug.Log($"Saving as JSON: {json}");
        }
        catch
        {
            throw new System.IO.FileNotFoundException();
        }

        return JsonUtility.FromJson<T>(json);
    }
}
[System.Serializable]
public abstract class SaveData { }