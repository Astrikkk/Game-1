using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : Vechicle
{

    void Update()
    {
        if (IsInCar == true)
        {
            moveInput = Input.GetAxis("Vertical");
            rotateInput = Input.GetAxis("Horizontal");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Sit();
        }
    }

    void FixedUpdate()
    {
        if (IsInCar == true)
        {
            transform.Translate(Vector2.up * moveInput * moveSpeed * Time.fixedDeltaTime);
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                transform.Rotate(Vector3.forward, -rotateInput * rotateSpeed * Time.fixedDeltaTime);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (CompareTag("Player"))IsColWithPlayer = true;
        else IsColWithPlayer = false;
    }
}
