using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : Health
{
    public int moveSpeed = 5;
    public GameObject Tower;
    private GameObject player;
    public GameObject Turret;
    [SerializeField] private float distanceToReact = 5f;
    [SerializeField] private List<Transform> patrolPoints;
    private int currentPatrolPointIndex;
    private bool isPatrollingForward = true;
    public bool IsPatroling;

    public float fireRate = 0.1f;    //Tower
    public int bulletsPerShot = 1;
    public float bulletSpread = 0.1f;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public Transform firePoint;
    private float fireTimer = 0f;
    public int maxAmmo = 10;
    public float reloadTime = 2f;
    private int currentAmmo;
    private bool isReloading = false;
    public float rotationSpeed = 6f;
    public GameObject broken;
    public GameObject obj;



    public float fireRateTurret = 0.1f;    //Turret
    public int bulletsPerShotTurret = 1;
    public float bulletSpreadTurret = 0.1f;
    public GameObject bulletPrefabTurret;
    public float bulletSpeedTurret = 20f;
    public Transform firePointTurret;
    private float fireTimerTurret = 0f;
    public int maxAmmoTurret = 10;
    public float reloadTimeTurret = 2f;
    private int currentAmmoTurret;
    private bool isReloadingTurret = false;
    private float distanceToEnemy;
    public bool TurretActive = false;


    public float rotateSpeed = 100f;
    public float moveDuration = 1f;
    private bool isMoving = false;
    private float moveTimer = 0f;

    private int randomDirection;
    private Quaternion targetRotation;
    void Update()
    {
    }
    void FixedUpdate()
    {
        player = CheckNearestObjec("Ukr");
        if (player != null)
        {
            distanceToEnemy = Vector2.Distance(transform.position, player.transform.position);


            if (distanceToEnemy <= distanceToReact)
            {
                RotateToObject();
                TurretShoot();
                Shoot();
            }
            else
            {
                if (IsPatroling) Patrol();
                else Move();
            }
        }

    }
    public void RotateToObject()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Ukr");
        List<float> distances = new List<float>();

        foreach (GameObject player in players)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            distances.Add(distance);
        }

        float minDistance = Mathf.Min(distances.ToArray());
        int minIndex = distances.IndexOf(minDistance);
        GameObject nearestPlayer = players[minIndex];

        Vector3 direction = nearestPlayer.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Tower.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Turret.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    GameObject CheckNearestObjec(string Tag)
    {
        GameObject check = GameObject.FindGameObjectWithTag(Tag);
        if (check == null) return null;
        GameObject[] players = GameObject.FindGameObjectsWithTag(Tag);
        List<float> distances = new List<float>();


        foreach (GameObject player in players)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            distances.Add(distance);
        }

        float minDistance = Mathf.Min(distances.ToArray());
        int minIndex = distances.IndexOf(minDistance);
        GameObject nearestPlayer = players[minIndex];
        return nearestPlayer;
    }
    private void Patrol()
    {
        if (patrolPoints.Count == 0) return;

        Transform currentPatrolPoint = patrolPoints[currentPatrolPointIndex];

        Vector2 direction = currentPatrolPoint.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        transform.position = Vector2.MoveTowards(transform.position, currentPatrolPoint.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, currentPatrolPoint.position) < 0.1f)
        {
            currentPatrolPointIndex++;

            if (currentPatrolPointIndex >= patrolPoints.Count)
            {
                IsPatroling = false;
            }
        }

    }
    public void Shoot()
    {
        if (isReloading) return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if (Time.time - fireTimer < fireRate) return;

        fireTimer = Time.time;
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }

        currentAmmo--;
    }
    private IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;

    }
    private void Move()
    {
        if (isMoving && IsPatroling)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

            moveTimer += Time.deltaTime;

            if (moveTimer >= moveDuration)
            {
                isMoving = false;
                moveTimer = 0f;
                targetRotation = Quaternion.Euler(0, 0, randomDirection);
            }
        }
        else
        {
            moveTimer += Time.deltaTime;

            if (moveTimer >= moveDuration)
            {
                isMoving = true;
                moveTimer = 0f;
                randomDirection = Random.Range(0, 360);
            }
        }

        if (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    public void TurretShoot()
    {
        if (isReloadingTurret) return;
        if (TurretActive == false) return;

        if (currentAmmoTurret <= 0)
        {
            StartCoroutine(ReloadTurret());
            return;
        }
        if (Time.time - fireTimerTurret < fireRateTurret) return;

        fireTimerTurret = Time.time;

        float accuracy = 1 - bulletSpreadTurret;

        for (int i = 0; i < bulletsPerShotTurret; i++)
        {
            float angle = Random.Range(-1f, 1f) * (1 - accuracy) * 90f;

            GameObject bullet = Instantiate(bulletPrefabTurret, firePointTurret.position, firePointTurret.rotation);

            bullet.transform.Rotate(0, 0, angle);

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.AddForce(firePointTurret.up * bulletSpeedTurret, ForceMode2D.Impulse);
        }

        currentAmmoTurret--;
    }
    private IEnumerator ReloadTurret()
    {
        isReloadingTurret = true;

        yield return new WaitForSeconds(reloadTimeTurret);

        currentAmmoTurret = maxAmmoTurret;
        isReloadingTurret = false;

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
        obj.SetActive(false);
    }
}