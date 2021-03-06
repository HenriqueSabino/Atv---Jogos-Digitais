using System.Collections;
using UnityEngine;

public class Enemy : PooledObject
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    new private Rigidbody2D rigidbody;

    // Stats
    public EnemyStatsSO stats;
    private float speed;
    private float jumpVel;
    private int health;
    private int damage;
    private bool canJump;
    private Transform target;
    private bool frozen;

    private void Start()
    {
        target = Player.instance.transform;

        speed = stats.speed;
        jumpVel = stats.jumpVel;
        health = stats.health;
        damage = stats.damage;
        animator.runtimeAnimatorController = stats.animController;
    }

    private void OnEnable()
    {
        if (stats != null)
        {
            speed = stats.speed;
            jumpVel = stats.jumpVel;
            health = stats.health;
            damage = stats.damage;
            animator.runtimeAnimatorController = stats.animController;
        }
    }

    public void ApplyStats(int damageBonus = 1)
    {
        if (stats != null)
        {
            speed = stats.speed;
            jumpVel = stats.jumpVel;
            health = stats.health;
            damage = stats.damage * damageBonus;
            animator.runtimeAnimatorController = stats.animController;
        }
    }

    public void ResetFreeze()
    {
        frozen = false;
        StartCoroutine(ResetFreezeCoroutine());
    }

    public IEnumerator ResetFreezeCoroutine()
    {
        animator.SetTrigger("Unfreeze");

        // Waiting for next frame
        yield return null;

        animator.ResetTrigger("Unfreeze");
        animator.ResetTrigger("Freeze");
    }

    private void FixedUpdate()
    {
        Vector3 dir = target.position - transform.position;

        float velX = 0;

        if (!frozen)
        {
            if (Mathf.Abs(dir.x) > 0.1)
            {
                velX = Mathf.Sign(dir.x) * speed;

                spriteRenderer.flipX = velX > 0;
            }

            if (dir.y > 3 && Mathf.Abs(dir.x) < 3 && canJump)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpVel);
                canJump = false;
            }
        }

        rigidbody.velocity = new Vector2(velX, rigidbody.velocity.y);
    }

    public void Freeze()
    {
        if (!frozen)
        {
            frozen = true;
            animator.SetTrigger("Freeze");
        }
    }

    public void Unfreeze()
    {
        if (frozen)
        {
            frozen = false;
            animator.SetTrigger("Unfreeze");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            health -= Player.instance.damage;

            if (health <= 0)
            {
                gameObject.SetActive(false);
                Player.instance.AddScore(1);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            rigidbody.velocity = Vector2.zero;
            Player.instance.Damage(damage);
        }
    }
}