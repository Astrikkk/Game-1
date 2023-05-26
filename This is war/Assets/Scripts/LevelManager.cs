using System.IO;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

public class LevelManager : MonoBehaviour
{
    private bool[] levelsPassed = new bool[10]; // Припустимо, у вас є 10 рівнів

    private static string saveFilePath = "progress.json"; // Шлях до файлу збереження

    public void MarkLevelAsPassed(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelsPassed.Length)
        {
            levelsPassed[levelIndex] = true;
        }
    }

    public bool IsLevelPassed(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelsPassed.Length)
        {
            return levelsPassed[levelIndex];
        }

        return false;
    }

    public void SaveProgress()
    {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(bool[]));

        using (FileStream fs = new FileStream(saveFilePath, FileMode.Create))
        {
            serializer.WriteObject(fs, levelsPassed);
        }
    }

    public void LoadProgress()
    {
        if (File.Exists(saveFilePath))
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(bool[]));

            using (FileStream fs = new FileStream(saveFilePath, FileMode.Open))
            {
                levelsPassed = (bool[])serializer.ReadObject(fs);
            }
        }
    }
}
