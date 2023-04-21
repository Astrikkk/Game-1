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
        if (Input.GetKeyDown(KeyCode.F) && IsInCar == true)
        {
            Sit();
        }
        if (Input.GetKeyDown(KeyCode.F) && IsColWithPlayer == true)
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
        if (collision.gameObject.CompareTag("Player")) IsColWithPlayer = true;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) IsColWithPlayer = false;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet")|| other.gameObject.CompareTag("EnemyBullet"))
        {
            HP -= other.gameObject.GetComponent<Bullet>().damage;
            if (HP <= 0) Kill();
        }
    }
    private void Kill()
    {
        broken.SetActive(true);
        broken.transform.position = obj.transform.position;
        broken.transform.rotation = obj.transform.rotation;
        if (IsInCar)
        {
            player.GetComponent<Player>().TakeDamage(99);
            Sit();
        }
        obj.SetActive(false);
    }
}
