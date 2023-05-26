using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mortar : MonoBehaviour
{
    public float Renge = 100;
    public GameObject Explosion;
    public bool CanShoot = false;
    public bool PlayerHere = false;
    private GameObject player;
    private float lastShootTime;
    public float ShootTime = 5f;
    private float distanceOfFire;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        distanceOfFire = ArtilerySlider.SliderValue;
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (CanShoot == false && PlayerHere)
                {
                    CanShoot = true;
                    player.GetComponent<Player>().CanMove = false;
                    player.GetComponent<Player>().UsingArtilery = true;
                }
                else
                {
                    CanShoot = false;
                    player.GetComponent<Player>().CanMove = true;
                    player.GetComponent<Player>().UsingArtilery = false;

                }
            }
            if (Input.GetMouseButton(0) && CanShoot == true && Time.time - lastShootTime >= 5f)
            {
                Shoot(); lastShootTime = Time.time;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.rotation *= Quaternion.Euler(0f, 0f, 0.3f);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.rotation *= Quaternion.Euler(0f, 0f, -0.3f);
            }
    }
    public void Shoot()
    {
        Vector3 spawnPos = transform.position + transform.up * (Renge * distanceOfFire);
        Quaternion spawnRot = transform.rotation;
        GameObject bullet = Instantiate(Explosion, spawnPos, spawnRot);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && CanShoot == false)
        {
            PlayerHere = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && CanShoot == false)
        {
            PlayerHere = false;
        }
    }
}
