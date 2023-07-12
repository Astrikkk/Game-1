using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public GameObject Explosion;
    public float ActiveTime = 5f;

    private void Start()
    {
        Invoke("BeActive", ActiveTime);
    }
    private void BeActive()
    {
        ActiveTime = 0;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ActiveTime == 0)
        {
            Health health = other.gameObject.GetComponent<Health>();
            if (health != null)
            {
                StartCoroutine("TS");
            }
        }
    }
    private IEnumerator TS()
    {
        Explosion.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
