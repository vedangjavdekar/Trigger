using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SerializationManager
{
    private static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        return formatter;
    }
    private static string GeneratePathString(string filename) => Application.persistentDataPath + "/saves/" + filename + ".data";

    public static bool Save(string filename, object saveData)
    {
        BinaryFormatter formatter = GetBinaryFormatter();

        if (!Directory.Exists(Application.persistentDataPath + "/saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }

        string path = GeneratePathString(filename);

        FileStream file = File.Create(path);
        formatter.Serialize(file, saveData);

        file.Close();

        return true;
    }
    public static object Load(string filename)
    {
        string filePath = GeneratePathString(filename);

        if (!File.Exists(filePath))
        {
            return null;
        }

        BinaryFormatter formatter = GetBinaryFormatter();

        FileStream file = File.Open(filePath, FileMode.Open);

        try
        {
            object savedData = formatter.Deserialize(file);
            file.Close();
            return savedData;
        }
        catch
        {
            Debug.LogError($"Failed to load file at {filePath}");
            file.Close();
            return null;
        }

    }
}
