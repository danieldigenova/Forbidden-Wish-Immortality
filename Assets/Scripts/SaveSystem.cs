using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/**
 * Script to save and load player and game progress data from disc
 */
public static class SaveSystem 
{
    // Function to save player progress data to disk
    public static void SavePlayer(PlayerController player)
    {
        // Save PlayerData to a file
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.fun";

        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    // Function to save game progress data to disk
    public static void SaveGame(GameController game)
    {
        // Save GameData to a file
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/game.fun";

        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(game);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    // Function to load player progress data from disc
    public static PlayerData LoadPlayer()
    {
        // Load data from file if exists
        string path = Application.persistentDataPath + "/player.fun";
        if (File.Exists(path))
        {
            // Load the data as a PlayerData
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not find in "+ path);
            return null;
        }
    }

    // Function to load game progress data from disc
    public static GameData LoadGame()
    {
        // Load data from file if exists
        string path = Application.persistentDataPath + "/game.fun";
        if (File.Exists(path))
        {
            // Load the data as a GameData
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameData data = formatter.Deserialize(stream) as GameData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not find in " + path);
            return null;
        }
    }
}
