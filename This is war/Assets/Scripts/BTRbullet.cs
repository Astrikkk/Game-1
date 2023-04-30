using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTRbullet : MonoBehaviour
{
    public bool FirstCol = true;
    public int speed = 30;
    public int damage = 20;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("GoThrought")) return;
        if (FirstCol == true)
        {
            FirstCol = false;
            return;

        }
        Health health = other.gameObject.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
