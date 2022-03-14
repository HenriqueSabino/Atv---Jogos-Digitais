using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance;

    [Header("Stats")]
    public float speed = 5f;
    public float jumpVel = 5f;
    public int maxHealth;
    public int health { get; private set; }
    [SerializeField]
    private int invencibilityTime;
    public int score { get; private set; }


    [Header("Shooting")]
    [SerializeField]
    private GameObject Gun;
    private float gunDist;

    [SerializeField]
    private GameObject bulletObject;
    public int maxBullets;
    public int bullets;
    public int damage;
    [SerializeField]
    private int shootingInterval;
    [SerializeField]
    private int rechargeTime;
    private bool recharging = false;
    private bool canShoot = true;
    private Pool bulletPool;

    [Header("Events")]
    public UnityEvent bulletCountChanged;
    public UnityEvent healthChanged;
    public UnityEvent scoreChanged;

    [Header("Components")]
    new public Collider2D collider;
    new public Rigidbody2D rigidbody;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer gunSpriteRenderer;

    private float horizontalVel = 0f;
    private bool canJump = false;
    private bool invencible = false;
    private bool dead = false;

    private void Awake()
    {
        instance = this;
        health = maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        bulletPool = new Pool(transform, bulletObject, maxBullets);
        score = 0;
        gunDist = (transform.position - Gun.transform.position).magnitude / transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            horizontalVel = Input.GetAxis("Horizontal") * speed;
            animator.SetFloat("Speed", Mathf.Abs(horizontalVel));

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
                    bulletCountChanged.Invoke();
                    ShootCoolDown();
                }
                else if (!recharging)
                {
                    Recharge();
                }
            }
        }
        else
        {
            horizontalVel = 0;
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

        if (!dead)
        {
            gunSpriteRenderer.flipY = angle <= -90 || angle >= 90;
            spriteRenderer.flipX = angle <= -90 || angle >= 90;

            if (spriteRenderer.flipX && horizontalVel > 0 || !spriteRenderer.flipX && horizontalVel < 0)
                animator.SetBool("backwards", true);
            else
                animator.SetBool("backwards", false);
        }

        if (transform.position.y <= -5)
        {
            transform.position = new Vector2(8.5f, 4.5f);
            Damage(1);
        }
    }

    private async void Invencible()
    {
        invencible = true;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.1f);
        await Task.Delay(invencibilityTime);
        invencible = false;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
    }

    private async void ShootCoolDown()
    {
        await Task.Delay(shootingInterval);
        canShoot = !recharging;
    }

    private async void Recharge()
    {
        recharging = true;
        canShoot = false;
        await Task.Delay(rechargeTime);
        bulletPool.ResetPool();

        bullets = maxBullets;
        bulletCountChanged.Invoke();
        canShoot = true;
        recharging = false;
    }

    public void Damage(int amount)
    {
        if (!invencible && !dead)
        {
            health -= amount;

            if (health <= 0)
            {
                dead = true;
                animator.SetTrigger("Die");
            }
            else
            {
                Invencible();
            }

            healthChanged.Invoke();
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Heal(int amount)
    {
        health += amount;

        if (health > maxHealth)
            health = maxBullets;

        healthChanged.Invoke();
    }

    public void AddScore(int points)
    {
        score += points;

        scoreChanged.Invoke();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            for (int i = 0; i < other.contactCount; i++)
            {
                ContactPoint2D contact = other.GetContact(i);

                if (contact.normal.normalized == Vector2.up)
                {
                    canJump = true;
                    break;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            canJump = false;
        }
    }
}
