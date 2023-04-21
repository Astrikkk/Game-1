using UnityEngine;

public class Player : Health
{
    public float moveSpeed = 5.0f;
    private Camera mainCamera;
    public GameObject gunPrefab;
    public GameObject HoldPoint;
    public GameObject ThrowPoint;
    private GameObject gun;
    public GameObject GranatePrefab;
    public int GranateCount;



    void Start()
    {
        mainCamera = Camera.main;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Gun"))
        {
            if (gun != null) return;
            gun = other.gameObject;
            gun.transform.SetParent(transform);
            gun.transform.position = HoldPoint.transform.position;
            gun.transform.rotation = HoldPoint.transform.rotation;
            gun.GetComponent<Rigidbody2D>().isKinematic = true;
            gun.GetComponent<Pistol>().IsInArms = true;
        }

        if (other.gameObject.CompareTag("Ammo"))
        {
            if (gun == null) return;
            gun.GetComponent<Pistol>().allAmmo += 20;
            GameObject ammo;
            ammo = other.gameObject;
            Destroy(ammo);
        }
    }
    void Shoot()
    {
        gun.GetComponent<Pistol>().Shoot();
    }
    void GranateThrow()
    {
        GameObject granate = Instantiate(GranatePrefab, ThrowPoint.transform.position, transform.rotation);
        Rigidbody2D bulletRb = granate.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(transform.right * 10, ForceMode2D.Impulse);
    }

    void Update()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(horizontal, vertical).normalized;
        transform.position += new Vector3(direction.x, direction.y, 0.0f) * moveSpeed * Time.deltaTime;

        Vector3 lookDirection = mousePosition - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90.0f;
        transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

        if (Input.GetMouseButton(0) && gun != null)
        {
            Shoot();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && gun != null)
        {
            DropGun();
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            GranateThrow();
        }
    }
    void DropGun()
    {
        if (gun != null)
        {
            gun.transform.parent = null;
            gun.GetComponent<Rigidbody2D>().isKinematic = false;
            gun.GetComponent<BoxCollider2D>().enabled = true;
            gun.GetComponent<Pistol>().IsInArms = false;
            gun = null;
        }
    }
}