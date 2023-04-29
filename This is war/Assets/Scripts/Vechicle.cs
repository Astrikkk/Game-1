using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vechicle : Health
{
    public float moveSpeed = 10f;
    public float rotateSpeed = 100f;
    protected float moveInput;
    protected float rotateInput;
    public GameObject broken;
    public GameObject obj;
    public Transform Exit;
    protected GameObject player;
    public bool IsInCar;
    protected bool IsColWithPlayer;
    public GameObject PlayerPoint;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }
    virtual protected void Sit()
    {
        if (IsInCar == true)
        {
            player.SetActive(true);
            player.transform.position = Exit.position;
            IsInCar = false;
            PlayerPoint.SetActive(false);
            return;
        }
        else
        {
            player.SetActive(false);
            IsInCar = true;
            PlayerPoint.SetActive(true);
            return;
        }
    }
    public override void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0) Kill();
    }
    protected virtual void Kill()
    {
        broken.SetActive(true);
        broken.transform.position = obj.transform.position;
        broken.transform.rotation = obj.transform.rotation;
        if (IsInCar)
        {
            player.GetComponent<Player>().TakeDamage(99);
            Sit();
        }
        obj.SetActive(false);
    }
}
