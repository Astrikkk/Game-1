using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.GetComponent<Player>().HP < 100)
            {
                collision.GetComponent<Player>().HP = 100;
                collision.GetComponent<Player>().mainCamera.GetComponent<CameraController>().PostProcessing.enabled = false;
                Destroy(gameObject);
            }
        }
    }
}
