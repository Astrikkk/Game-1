using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Vechicle
{
    public Camera mainCamera;
    public GameObject Tower;
    private GameObject enemy;
    public GameObject Turret;
    [SerializeField] private float distanceToReact = 5f;

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
    public float rotationSpeed = 6f;


    public float fireRateTurret = 0.1f;
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

    public GameObject ShootExplosion;
    public AudioClip shoot;


    private void Start()
    {
        mainCamera = Camera.main;
        _audio = GetComponent<AudioSource>();
        currentAmmo = maxAmmo;
    }
    void Update()
    {
        if (IsInCar == true)
        {
            _audio.volume = 10;
            moveInput = Input.GetAxis("Vertical");
            rotateInput = Input.GetAxis("Horizontal");
            if (Input.GetMouseButton(0))
            {
                Shoot();
            }
            if (Input.GetKeyUp(KeyCode.T))
            {
                TurretActive = !TurretActive;
            }
        }
        else
        {
            _audio.volume = 1;
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
            GameObject shootExplosion = Instantiate(ShootExplosion, firePoint.position, firePoint.rotation);
            _audio.PlayOneShot(shoot);
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
    void FixedUpdate()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (IsInCar == true)
        {
            transform.Translate(Vector2.up * moveInput * moveSpeed * Time.fixedDeltaTime);
            transform.Rotate(Vector3.forward, -rotateInput * rotateSpeed * Time.fixedDeltaTime);
            Vector3 lookDirection = mousePosition - transform.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90.0f;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Tower.transform.rotation = Quaternion.Lerp(Tower.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            if (enemy != null)
            {
                distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                RotateToObject();



                if (distanceToEnemy <= distanceToReact)
                {
                    TurretShoot();
                }
            }



        }
    }
    public void RotateToObject()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Enemy");
        List<float> distances = new List<float>();

        foreach (GameObject player in players)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            distances.Add(distance);
        }

        float minDistance = Mathf.Min(distances.ToArray());
        int minIndex = distances.IndexOf(minDistance);
        GameObject nearestPlayer = players[minIndex];

        Vector3 direction = nearestPlayer.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Turret.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) IsColWithPlayer = true;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) IsColWithPlayer = false;
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

            bullet.transform.Rotate(0, 0, angle + 90);

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
}
