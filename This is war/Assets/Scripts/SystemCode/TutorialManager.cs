using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject Tutorial;

    private void Start()
    {
        Instantiate(Tutorial, transform);
        Tutorial.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            if (Tutorial.activeSelf)
            {
                CloseTutorial();
            }
            else
            {
                ShowTutorial();
            }
        }
    }

    public void ShowTutorial()
    {
        Tutorial.SetActive(true);
    }

    public void CloseTutorial()
    {
        Tutorial.SetActive(false);
    }
}
