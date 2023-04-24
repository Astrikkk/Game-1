using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBullet : MonoBehaviour
{
    public GameObject particles;
    public GameObject hit;
    public bool FirstCol = true;

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
        hit.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
