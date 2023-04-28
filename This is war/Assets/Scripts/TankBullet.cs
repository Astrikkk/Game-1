using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBullet : MonoBehaviour
{
    public GameObject particles;
    public bool FirstCol = true;
    public int speed = 30;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (FirstCol == true)
        {
            FirstCol = false;
            return;

        }
        StartCoroutine("TS");
    }
    private IEnumerator TS()
    {
        particles.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
