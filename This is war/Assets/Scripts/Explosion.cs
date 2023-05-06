using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    void Start()
    {
        StartCoroutine("TS");
    }
    private IEnumerator TS()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
