using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Health
{
    private Collider2D _collider;
    public List<GameObject> Broken = new List<GameObject>();
    public int BrokeDamage = 500;
    private bool destroyed = false;


    private void Start()
    {
        _collider = GetComponent<Collider2D>();
    }



    public override void TakeDamage(int damage)
    {
        if(!(destroyed)){
            HP -= damage;
            if (Broken.Count == 0)
            {
                if (HP <= 0)
                {
                    Destroy(gameObject);
                    _collider.enabled = false;
                    destroyed = true;
                }
            }
            else if (Broken.Count > 2 && Broken.Count < 4)
            {
                if (HP <= BrokeDamage)
                {
                    Broken[0].SetActive(false);
                    Broken[1].SetActive(true);
                }
                if (HP <= 0)
                {
                    Broken[1].SetActive(false);
                    Broken[0].SetActive(false);
                    Broken[2].SetActive(true);
                    _collider.enabled = false;
                    destroyed = true;
                }
                return;
            }
            else if (Broken.Count > 1 && Broken.Count < 3)
            {
                if (HP <= 0)
                {
                    Broken[0].SetActive(false);
                    Broken[1].SetActive(true);
                    _collider.enabled = false;
                    destroyed = true;
                    return;
                }
            }
        }
    }
}