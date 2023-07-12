using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranateHit : MonoBehaviour
{
    public int damage = 100;

    private void OnCollisionEnter2D(Collision2D other)
    {

        Health health = other.gameObject.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damage);

        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Health health = other.gameObject.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damage);

        }
    }
}
