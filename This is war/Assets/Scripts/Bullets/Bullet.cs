using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{

    public float speed = 30f;
    public int damage = 10;
    private float collisionDelay = 0.01f;
    private bool canCollide = false;
    public string nameTag;

    void Start()
    {
        Invoke(nameof(EnableCollision), collisionDelay);
    }
    // При стиканні з іншим об'єктом, проводимо дії
    //void OnTriggerEnter2D(Collider2D hitInfo)
    //{
    //    // Отримуємо компонент здоров'я об'єкту
    //    Enemy enemy = hitInfo.GetComponent<Enemy>();
    //    // Якщо об'єкт має компонент здоров'я, завдаємо йому шкоду
    //    if (enemy != null)
    //    {
    //        enemy.TakeDamage(damage);
    //    }
    //    // Знищуємо кулю
    //    Destroy(gameObject);
    //}
    void FixedUpdate()
    {
        if (!canCollide)
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Default"), true);
        }
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
    void EnableCollision()
    {
        canCollide = true;
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Default"), false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!canCollide)
        {
            return;
        }
        if (other.gameObject.CompareTag(nameTag))
        {
            return;
        }
        if (other.gameObject.CompareTag("GoThrought")) return;
        Health health = other.gameObject.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Health health = other.gameObject.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}

