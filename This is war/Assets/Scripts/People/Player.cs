using UnityEngine;

public class Player : Health
{
    public float moveSpeed = 5.0f;
    public Camera mainCamera;
    public GameObject HoldPoint;
    public GameObject ThrowPoint;
    private GameObject gun;
    public GameObject GranatePrefab;
    public int GranateCount;
    public bool CanMove = true;
    public bool UsingArtilery = false;
    private Animator Animation;

    private bool isMoving;
    private bool isWithGun;

    void Start()
    {
        mainCamera = Camera.main;
        Animation = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Gun"))
        {
            if (gun != null) return;
            gun = other.gameObject;
            if (gun.GetComponent<Pistol>().CanBePickedUp == false)
            {
                gun = null;
                return;
            }
            gun.transform.SetParent(transform);
            gun.transform.position = HoldPoint.transform.position;
            gun.transform.rotation = HoldPoint.transform.rotation;
            gun.GetComponent<Rigidbody2D>().isKinematic = true;
            gun.GetComponent<Pistol>().IsInArms = true;
            gun.GetComponent<Pistol>().CanBePickedUp = false;
            gun.GetComponent<SpriteRenderer>().sprite = gun.GetComponent<Pistol>().skin[1];

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
        if (GranateCount <= 0) return;
        GameObject granate = Instantiate(GranatePrefab, ThrowPoint.transform.position, transform.rotation);
        Rigidbody2D bulletRb = granate.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(transform.up * 10, ForceMode2D.Impulse);
        GranateCount--;
    }

    void Update()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (CanMove)
        {
            isWithGun = (gun != null);
            isMoving = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0);

            Animation.SetBool("IsWithGun", isWithGun);
            Animation.SetBool("IsMoving", isMoving);
            Vector2 direction = new Vector2(horizontal, vertical).normalized;
            transform.position += new Vector3(direction.x, direction.y, 0.0f) * moveSpeed * Time.deltaTime;

            if (isMoving)
            {
                if (gun == null)
                {
                    // ³��������� �������� ��� ����, ���� gun == null
                    Animation.Play("ZSU1Anim without gun 1");
                }
                else
                {
                    // ³��������� ���� �������� ��� ����, ���� gun != null
                    Animation.Play("ZSU1Anim with gun");
                }
            }
            else
            {
                Animation.StopPlayback();
            }

            Vector3 lookDirection = mousePosition - transform.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90.0f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

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
    }
    void DropGun()
    {
        if (gun != null)
        {
            gun.transform.parent = null;
            gun.GetComponent<Rigidbody2D>().isKinematic = false;
            gun.GetComponent<BoxCollider2D>().enabled = true;
            gun.GetComponent<Pistol>().IsInArms = false;
            gun.GetComponent<SpriteRenderer>().sprite = gun.GetComponent<Pistol>().skin[0];
            gun.GetComponent<Pistol>().IvokePickUp();
            gun = null;
        }
    }
    public override void TakeDamage(int damage)
    {
        if (HP <= 50)
        {
            mainCamera.GetComponent<CameraController>().PostProcessing.enabled = true;
        }
        else
        {

            mainCamera.GetComponent<CameraController>().PostProcessing.enabled = false;
        }
        base.TakeDamage(damage);
    }
}
