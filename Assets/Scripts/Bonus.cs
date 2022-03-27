using System.Collections;
using UnityEngine;

public class Bonus : PooledObject
{
    public new SpriteRenderer renderer;
    public BonusSO bonusSO;

    void Start()
    {
        renderer.sprite = bonusSO.sprite;
    }

    void OnEnable()
    {
        if (bonusSO != null)
            renderer.sprite = bonusSO.sprite;
        StartCoroutine(DisableAfterSeconds(3));
    }

    public void ApplyStats()
    {
        if (bonusSO != null)
            renderer.sprite = bonusSO.sprite;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (bonusSO.health != 0)
            {
                Player.instance.Heal(bonusSO.health);
            }

            if (bonusSO.maxHealth != 0)
            {
                Player.instance.IncreaseMaxHealth(bonusSO.maxHealth);
            }

            if (bonusSO.maxBullets != 0)
            {
                Player.instance.IncreaseMaxBullets(bonusSO.maxBullets);
            }


            gameObject.SetActive(false);
        }
    }

    IEnumerator DisableAfterSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
}
