using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class DataManager{
    public static GameData savedGame;

    public static void SaveData()
    {
        savedGame = GameData.current;
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/data.sav");
        binaryFormatter.Serialize(file, savedGame);
        file.Close();
    }

    public static void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/data.sav"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/data.sav", FileMode.Open);
            savedGame = (GameData)binaryFormatter.Deserialize(file);
            file.Close();
            GameData.current = savedGame;
        }
    }
}
