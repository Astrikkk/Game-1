using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Health
{
    private GameObject player;
    [SerializeField] private float distanceToReact = 5f;
    [SerializeField] private List<Transform> patrolPoints;
    public int moveSpeed = 5;
    private int currentPatrolPointIndex;
    private bool isPatrollingForward = true;
    private bool isStunned = false;
    public bool IsPatroling;

    public float rotateSpeed = 100f;
    public float moveDuration = 1f;

    private bool isMoving = false;
    private float moveTimer = 0f;


    public float fireRate = 0.1f;
    public int bulletsPerShot = 1;
    public float bulletSpread = 0.1f;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public Transform firePoint;
    private float fireTimer = 0f;
    public int maxAmmo = 10;
    public float reloadTime = 2f;
    private int currentAmmo;
    private bool isReloading = false;


    private int randomDirection;

    private void Start()
    {
        isMoving = true;
    }

    private void FixedUpdate()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (!isStunned)
        {
            if (player != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
                if (distanceToPlayer <= distanceToReact)
                {
                    Shoot();
                    Vector3 lookDirection = player.transform.position - transform.position;
                    float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90.0f;
                    transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
                }
                else
                {
                    if (IsPatroling) Patrol();
                    else Move();
                }
            }
        }
    }
    private void Move()
    {
        if (isMoving)
        {
            
            // переміщення в рандомному напрямку
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

            // таймер переміщення
            moveTimer += Time.deltaTime;

            // якщо час переміщення вичерпано
            if (moveTimer >= moveDuration)
            {
                // зупинка та розвертання
                isMoving = false;
                moveTimer = 0f;
                transform.Rotate(0f, 0f, randomDirection);
            }
        }
        else
        {
            // якщо не рухається, таймер для очікування
            moveTimer += Time.deltaTime;

            // якщо час очікування вичерпано
            if (moveTimer >= moveDuration)
            {
                // продовження руху в новому рандомному напрямку
                isMoving = true;
                moveTimer = 0f;
                randomDirection = Random.Range(0, 360);
            }
        }
    }
    private void Patrol()
    {
        if (patrolPoints.Count == 0) return;

        Vector2 currentPatrolPoint = patrolPoints[currentPatrolPointIndex].position;
        transform.position = Vector2.MoveTowards(transform.position, currentPatrolPoint, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, currentPatrolPoint) < 0.1f)
        {
            if (isPatrollingForward)
            {
                currentPatrolPointIndex++;
                if (currentPatrolPointIndex >= patrolPoints.Count)
                {
                    currentPatrolPointIndex = Mathf.Max(0, patrolPoints.Count - 2);
                    isPatrollingForward = false;
                }
            }
            else
            {
                currentPatrolPointIndex--;
                if (currentPatrolPointIndex < 0)
                {
                    currentPatrolPointIndex = Mathf.Min(1, patrolPoints.Count - 1);
                    isPatrollingForward = true;
                }
            }
        }
    }
    public void Shoot()
    {
        if (isReloading) return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if (Time.time - fireTimer < fireRate) return;

        fireTimer = Time.time;

        float accuracy = 1 - bulletSpread;

        for (int i = 0; i < bulletsPerShot; i++)
        {
            float angle = Random.Range(-1f, 1f) * (1 - accuracy) * 90f;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            bullet.transform.Rotate(0, 0, angle);

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.AddForce(firePoint.right * bulletSpeed, ForceMode2D.Impulse);
        }

        currentAmmo--;
    }
    private IEnumerator Stun()
    {
        isStunned = true;
        yield return new WaitForSeconds(1f);
        isStunned = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Stun());
        }
        if (other.gameObject.CompareTag("Bullet"))
        {
            HP -= other.gameObject.GetComponent<Bullet>().damage;
            if (HP <= 0) Destroy(gameObject);
        }
    }
    private IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
