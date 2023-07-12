using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool IsInBattle = false;
    public GameObject WinMenu;
    public GameObject LooseMenu;
    private GameObject[] enemies;
    private GameObject player;
    public static int CurrentLVL = 0;
    public void LoadScene(int a)
    {
        CurrentLVL = LevelManager.CurrentLVL;
        SceneManager.LoadScene(a);
    }
    private void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        player = GameObject.FindGameObjectWithTag("Player");
        CurrentLVL = LevelManager.CurrentLVL;
    }
    private void FixedUpdate()
    {
        if (IsInBattle)
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            player = GameObject.FindGameObjectWithTag("Player");
            if (enemies.Length <= 0)
            {
                WinMenu.SetActive(true);
                PlayerPrefs.SetInt("IsGameWon", 1);
                PlayerPrefs.Save();
                IsInBattle = false;
            }
            if (player == null)
            {
                LooseMenu.SetActive(true);
                IsInBattle = false;
            }
        }
    }
    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = (currentIndex + 1) % SceneManager.sceneCountInBuildSettings;
        CurrentLVL = LevelManager.CurrentLVL;
        SceneManager.LoadScene(nextIndex);
    }

    public void ReloadScene()
    {
        CurrentLVL = LevelManager.CurrentLVL;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}