using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    // Saves a given object data to binary format
    public static void SaveData(int data)
    {
        
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/data.saveFile";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerHighScoreData saveData = new PlayerHighScoreData(data);
        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static PlayerHighScoreData LoadData()
    {
        string path = Application.persistentDataPath + "/data.saveFile";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerHighScoreData data = formatter.Deserialize(stream) as PlayerHighScoreData;
            stream.Close();

            return data;
           
        }
        else
        {
            Debug.LogError("File not found");
            return null;
        }
    }
}
