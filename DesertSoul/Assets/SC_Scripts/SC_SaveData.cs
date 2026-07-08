using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SC_SaveData
{
    public static void SavePlayer (SC_Player_Prop player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.wow";
        FileStream stream = new FileStream(path, FileMode.Create);

        SC_PlayerData data = new SC_PlayerData(player);

        formatter.Serialize(stream,data);
        stream.Close();
    }

    public static SC_PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.wow";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SC_PlayerData data = formatter.Deserialize(stream) as SC_PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save not found in " +path);
            return null;
        }
    }
}
