using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 10f;
    public int damage = 10;
    private bool isMoving;
    private float collisionDelay = 0.03f;
    private bool canCollide = false;


    void Start()
    {
        isMoving = true;
        Invoke(nameof(EnableCollision), collisionDelay);
    }
    // ��� ������� � ����� ��'�����, ��������� 䳿
    //void OnTriggerEnter2D(Collider2D hitInfo)
    //{
    //    // �������� ��������� ������'� ��'����
    //    Enemy enemy = hitInfo.GetComponent<Enemy>();
    //    // ���� ��'��� �� ��������� ������'�, ������� ���� �����
    //    if (enemy != null)
    //    {
    //        enemy.TakeDamage(damage);
    //    }
    //    // ������� ����
    //    Destroy(gameObject);
    //}
    void FixedUpdate()
    {
        if (!canCollide)
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Default"), true);
        }
    }
    void EnableCollision()
    {
        canCollide = true;
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Default"), false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!canCollide)
        {
            return;
        }
        Destroy(gameObject);
    }
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}

