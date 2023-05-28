using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

public class LevelManager : MonoBehaviour
{
    public bool[] levelsPassed = new bool[10]; // Припустимо, у вас є 10 рівнів
    public int CurrentLVL = 0;
    public string[] levelDescription = new string[10];
    public TextMeshProUGUI Description;
    public GameObject GameManager_;
    
    private static string saveFilePath = "progress.json"; // Шлях до файлу збереження


    public void ChooseLevel(int a)
    {
        CurrentLVL = a;
        a--;
        Description.text = levelDescription[a];
    }
    public void EnterLVL()
    {
        GameManager_.GetComponent<GameManager>().LoadScene(CurrentLVL);
    }
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
