using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granate : MonoBehaviour
{
    public GameObject particles;
    public GameObject hit;
    public int ExplosionTime;
   
    void Start()
    {
        StartCoroutine("TS");
    }
    private IEnumerator TS()
    {
        yield return new WaitForSeconds(ExplosionTime);
        particles.SetActive(true);
        hit.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
