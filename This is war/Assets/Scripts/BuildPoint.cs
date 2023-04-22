using System.Collections.Generic;
using UnityEngine;

public class BuildPoint : MonoBehaviour
{
    public GameObject BuildMenu;
    public bool CanBuild = true;
    public List<GameObject> buildedObjects;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenMenu();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Gun"))
            CanBuild = false;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Gun"))
            CanBuild = true;
    }

    public void OpenMenu()
    {
        BuildMenu.SetActive(true);

        Player player = null;
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player))
        {
            player.CanMove = false;
        }
    }

    public void CloseMenu()
    {
        BuildMenu.SetActive(false);

        Player player = null;
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player))
        {
            player.CanMove = true;
        }
    }

    public void BuildObj(int number)
    {
        if (!CanBuild || !buildedObjects.Contains(buildedObjects[number]))
        {
            CloseMenu();
            return;
        }

        Instantiate(buildedObjects[number], transform.position, transform.rotation).transform.SetParent(null);

        CloseMenu();
    }
}
