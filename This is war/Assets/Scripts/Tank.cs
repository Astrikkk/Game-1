using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Vechicle
{
    public Camera mainCamera;
    public GameObject Tower;

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
    void Update()
    {
        if (IsInCar == true)
        {
            moveInput = Input.GetAxis("Vertical");
            rotateInput = Input.GetAxis("Horizontal");
            if (Input.GetMouseButton(0))
            {
                Shoot();
            }
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

        float accuracy = 1 - bulletSpread;

        for (int i = 0; i < bulletsPerShot; i++)
        {
            float angle = Random.Range(-1f, 1f) * (1 - accuracy) * 90f;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            bullet.transform.Rotate(0, 0, angle);
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
        Tower.transform.position = transform.position;
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;
        if (IsInCar == true)
        {
            transform.Translate(Vector2.up * moveInput * moveSpeed * Time.fixedDeltaTime);
            transform.Rotate(Vector3.forward, -rotateInput * rotateSpeed * Time.fixedDeltaTime);
            Vector3 lookDirection = mousePosition - transform.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90.0f;
            Tower.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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

}
