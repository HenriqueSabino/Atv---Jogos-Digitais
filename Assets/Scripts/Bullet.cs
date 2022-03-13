using System.Collections;
using UnityEngine;

public class Bullet : PooledObject
{
    public float Speed = 10f;

    new private Rigidbody2D rigidbody;
    private Coroutine DeactivateCoroutine;

    protected override void Awake()
    {
        base.Awake();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rigidbody.velocity = transform.right * Speed;
        DeactivateCoroutine = StartCoroutine(Deactivate());
    }

    private void FixedUpdate()
    {
        float angle = Mathf.Atan2(rigidbody.velocity.y, rigidbody.velocity.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            gameObject.SetActive(false);
            StopCoroutine(DeactivateCoroutine);
        }
    }

    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(5);

        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
