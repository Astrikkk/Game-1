using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Health
{
    public GameObject wall;
    public GameObject Broken;
    public GameObject Destroyed;
    private Collider2D _collider;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    public override void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 500)
        {
            wall.SetActive(false);
            Broken.SetActive(true);
        }
        if (HP <= 0)
        {
            wall.SetActive(false);
            Broken.SetActive(false);
            Destroyed.SetActive(true);
            _collider.enabled = false;
        }
    }
}
