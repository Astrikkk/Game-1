using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.A))
        {
            Transform objectTransform = GetComponent<Transform>();
            objectTransform.Rotate(0, 90, 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Transform objectTransform = GetComponent<Transform>();
            objectTransform.Rotate(0, -90, 0);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            if (other.gameObject.GetComponent<Platform>().IsRed == true)
            {
                Destroy(gameObject);
            }
        }
    }
}
