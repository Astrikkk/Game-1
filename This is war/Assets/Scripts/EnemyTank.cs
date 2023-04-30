using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : Health
{
    public int moveSpeed = 5;
    public GameObject Tower;
    private GameObject player;
    public GameObject Turret;
    [SerializeField] private float distanceToReact = 5f;

    public float fireRate = 0.1f;    //Tower
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
    public float rotationSpeed = 6f;
    public GameObject broken;
    public GameObject obj;



    public float fireRateTurret = 0.1f;    //Turret
    public int bulletsPerShotTurret = 1;
    public float bulletSpreadTurret = 0.1f;
    public GameObject bulletPrefabTurret;
    public float bulletSpeedTurret = 20f;
    public Transform firePointTurret;
    private float fireTimerTurret = 0f;
    public int maxAmmoTurret = 10;
    public float reloadTimeTurret = 2f;
    private int currentAmmoTurret;
    private bool isReloadingTurret = false;
    private float distanceToEnemy;
    public bool TurretActive = false;


    public bool IsPatroling;
    public float rotateSpeed = 100f;
    public float moveDuration = 1f;
    private bool isMoving = false;
    private float moveTimer = 0f;

    private int randomDirection;
    void Update()
    {
    }
    void FixedUpdate()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            distanceToEnemy = Vector2.Distance(transform.position, player.transform.position);
            Vector3 lookDirection = player.transform.position - transform.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90.0f;
            Turret.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


            if (distanceToEnemy <= distanceToReact)
            {
                Vector3 lookDirection2 = player.transform.position - transform.position;
                float angle2 = Mathf.Atan2(lookDirection2.y, lookDirection2.x) * Mathf.Rad2Deg - 90.0f;
                Tower.transform.rotation = Quaternion.AngleAxis(angle2, Vector3.forward);
                TurretShoot();
                Shoot();
            }
            else
            {
                Move();
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
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }

        currentAmmo--;
    }
    private IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;

    }
    private void Move()
    {
        if (isMoving && IsPatroling)
        {

            // переміщення в рандомному напрямку
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);


            // таймер переміщення
            moveTimer += Time.deltaTime;

            // якщо час переміщення вичерпано
            if (moveTimer >= moveDuration)
            {
                // зупинка та розвертання
                isMoving = false;
                moveTimer = 0f;
                transform.Rotate(Vector3.forward, randomDirection * rotateSpeed * Time.fixedDeltaTime);
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
    public void TurretShoot()
    {
        if (isReloadingTurret) return;
        if (TurretActive == false) return;

        if (currentAmmoTurret <= 0)
        {
            StartCoroutine(ReloadTurret());
            return;
        }
        if (Time.time - fireTimerTurret < fireRateTurret) return;

        fireTimerTurret = Time.time;

        float accuracy = 1 - bulletSpreadTurret;

        for (int i = 0; i < bulletsPerShotTurret; i++)
        {
            float angle = Random.Range(-1f, 1f) * (1 - accuracy) * 90f;

            GameObject bullet = Instantiate(bulletPrefabTurret, firePointTurret.position, firePointTurret.rotation);

            bullet.transform.Rotate(0, 0, angle);

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.AddForce(firePointTurret.up * bulletSpeedTurret, ForceMode2D.Impulse);
        }

        currentAmmoTurret--;
    }
    private IEnumerator ReloadTurret()
    {
        isReloadingTurret = true;

        yield return new WaitForSeconds(reloadTimeTurret);

        currentAmmoTurret = maxAmmoTurret;
        isReloadingTurret = false;

    }

    public override void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0) Kill();
    }
    protected virtual void Kill()
    {
        broken.SetActive(true);
        broken.transform.position = obj.transform.position;
        broken.transform.rotation = obj.transform.rotation;
        obj.SetActive(false);
    }
}