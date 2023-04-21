using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int HP = 100;
    virtual public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0) Destroy(gameObject);
    }
}
