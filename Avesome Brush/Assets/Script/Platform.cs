using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Material redMaterial;
    public Material YellowMaterial;
    public float materialChangeDelay = 2f;

    private bool isPlayerTouched = false;
    public bool IsRed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (IsRed != true)
            {
                isPlayerTouched = true;
                ChangeMaterial(YellowMaterial);
                Invoke("ChangeMaterialDelayed", materialChangeDelay);
            }
        }
    }

    void ChangeMaterial(Material newMaterial)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = newMaterial;
        }
    }

    void ChangeMaterialDelayed()
    {
        if (isPlayerTouched)
        {
            ChangeMaterial(redMaterial);
            IsRed = true;
        }
    }
}
