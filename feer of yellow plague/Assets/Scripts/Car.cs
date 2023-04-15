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
        if (Input.GetKeyDown(KeyCode.F) && IsInCar==true)
        {
            Sit();
        }
        if (Input.GetKeyDown(KeyCode.F) && IsColWithPlayer==true)
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))IsColWithPlayer = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))IsColWithPlayer = false;
    }

}
