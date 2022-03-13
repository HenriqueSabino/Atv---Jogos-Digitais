using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float jumpVel = 5f;

    [Header("Shooting")]
    public GameObject Gun;
    private float gunDist;

    public GameObject bulletObject;
    public int maxBullets;
    public int bullets;
    public int shootingInterval;
    public int rechargeTime;
    public bool recharging = false;
    private bool canShoot = true;
    private Pool bulletPool;

    // Movement
    new private Rigidbody2D rigidbody;
    private float horizontalVel = 0f;
    private bool canJump = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        bulletPool = new Pool(transform, bulletObject, maxBullets);

        gunDist = (transform.position - Gun.transform.position).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalVel = Input.GetAxis("Horizontal") * speed;

        if (canJump && Input.GetButtonDown("Jump"))
        {
            rigidbody.velocity += Vector2.up * jumpVel;
            canJump = false;
        }

        if (!recharging && Input.GetKeyDown(KeyCode.R))
        {
            Recharge();
        }

        if (canShoot && Input.GetButton("Fire1"))
        {
            if (bullets > 0)
            {
                Vector2 shootDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
                float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;

                Quaternion rotation = Quaternion.Euler(0, 0, angle);

                GameObject obj = bulletPool.Spawn(Gun.transform.position, rotation);

                canShoot = false;
                bullets--;
                ShootCoolDown();
            }
            else if (!recharging)
            {
                Recharge();
            }
        }
    }

    void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(horizontalVel, rigidbody.velocity.y);

        Vector2 shootDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        shootDir.Normalize();
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        Gun.transform.localRotation = rotation;
        Gun.transform.localPosition = shootDir * gunDist;
    }

    private async void ShootCoolDown()
    {
        await Task.Delay(shootingInterval);
        canShoot = true;
    }

    private async void Recharge()
    {
        recharging = true;
        await Task.Delay(rechargeTime);
        bulletPool.ResetPool();

        bullets = maxBullets;
        canShoot = true;
        recharging = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            for (int i = 0; i < col.contactCount; i++)
            {
                ContactPoint2D contact = col.GetContact(i);

                if (contact.normal.normalized == Vector2.up)
                {
                    canJump = true;
                    break;
                }
            }
        }
    }
}
