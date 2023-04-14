using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vechicle : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotateSpeed = 100f;
    protected float moveInput;
    protected float rotateInput;
    public int HP;
    public GameObject broken;
    public GameObject obj;
    public Transform Exit;
    protected GameObject player;
    public bool IsInCar;
    protected bool IsColWithPlayer;
    public GameObject PlayerPoint;
    protected void Start()
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
}
