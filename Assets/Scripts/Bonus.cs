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
            if (!bonusSO.specialBonus)
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

                if (bonusSO.damage != 0)
                {
                    Player.instance.IncreaseDamage(bonusSO.damage);
                }
            }
            else
            {
                switch (bonusSO.specialBonusTag)
                {
                    case "Freeze":
                        FreezeEnemies();
                        break;
                    case "Clear":
                        ClearEnemies();
                        break;
                }
            }


            gameObject.SetActive(false);
        }
    }

    private void FreezeEnemies()
    {
        // Stoping enemy spawners
        foreach (var enemySpawner in EnemySpawner.instances)
        {
            enemySpawner.Freeze();
        }
    }

    private void ClearEnemies()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            // Clearing enemies
            enemy.gameObject.SetActive(false);
            Player.instance.AddScore(1);
        }
    }

    IEnumerator DisableAfterSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
}
