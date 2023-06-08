using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public bool IsInArms = false;
    public int maxAmmo = 10;
    public float reloadTime = 2f;
    public Transform firePoint;
    public int allAmmo = 50;
    public float fireRate = 0.1f;
    public int bulletsPerShot = 1;
    public float bulletSpread = 0.1f;
    public Sprite[] skin;
    public int damage;

    private float fireTimer = 0f;

    private int currentAmmo;
    private bool isReloading = false;
    public bool CanBePickedUp = true;
    private AudioSource _audio;
    public AudioClip shoot;

    private void Start()
    {
        currentAmmo = maxAmmo;
        _audio = GetComponent<AudioSource>();
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
            _audio.PlayOneShot(shoot);
            bullet.transform.Rotate(0, 0, angle + 90);
            bullet.GetComponent<Bullet>().damage = this.damage;
        }
        
        currentAmmo--;
    }
    public void IvokePickUp()
    {
        Invoke("CanPickUp", 3);
    }
    public void CanPickUp()
    {
        CanBePickedUp = !CanBePickedUp;
    }
    private IEnumerator Reload()
    {
        if (allAmmo > 0)
        {
            isReloading = true;

            yield return new WaitForSeconds(reloadTime);

            allAmmo += currentAmmo;
            allAmmo -= maxAmmo;
            currentAmmo = maxAmmo;
            isReloading = false;
        }
    }
}
