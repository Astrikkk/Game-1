using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPoint : MonoBehaviour
{
    public GameObject BuildMenu;
    public GameObject showBuild;
    public bool CanBuild = true;
    public List<GameObject> buildedObjects;
    public float buildDistance = 2.0f;
    public LayerMask collisionLayer;

    private GameObject currentObject;

    private void Update()
    {

        if (currentObject != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10.0f;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, worldPos - transform.position, buildDistance, collisionLayer);

            if (hit.collider != null && hit.collider.gameObject != currentObject)
            {
                Destroy(currentObject);
            }

            currentObject.transform.position = hit.point;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        CanBuild = false;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        CanBuild = true;
    }

    public void OpenMenu()
    {
        BuildMenu.SetActive(true);
        showBuild.SetActive(true);

        Player player = null;
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player))
        {
            player.CanMove = false;
        }
    }

    public void CloseMenu()
    {
        BuildMenu.SetActive(false);
        showBuild.SetActive(false);

        Player player = null;
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player))
        {
            player.CanMove = true;
        }

        if (currentObject != null)
        {
            Destroy(currentObject);
            currentObject = null;
        }
    }

    public void BuildObj(int number)
    {
        if (!CanBuild || !buildedObjects.Contains(buildedObjects[number]))
        {
            CloseMenu();
            return;
        }

        currentObject = Instantiate(buildedObjects[number], transform.position, Quaternion.identity);

        CloseMenu();
    }

    private IEnumerator DestroyObjectAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(obj);
    }
}