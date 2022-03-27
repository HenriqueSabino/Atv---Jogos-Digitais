using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public TMP_Text bulletsText;
    public TMP_Text scoreText;
    public TMP_Text waveText;
    public Slider HealthSlider;
    public TMP_Text HealthText;

    private void Start()
    {
        Player.instance.bulletCountChanged.AddListener(BulletsChanged);
        BulletsChanged();

        Player.instance.healthChanged.AddListener(HealthChanged);
        HealthChanged();

        Player.instance.scoreChanged.AddListener(ScoreChanged);
        ScoreChanged();

        GameController.instance.waveChanged.AddListener(WaveChanged);
        WaveChanged();
    }

    private void BulletsChanged()
    {
        bulletsText.text = $"Bullets: {Player.instance.bullets}/{Player.instance.maxBullets}";
    }

    private void HealthChanged()
    {
        HealthText.text = $"{Player.instance.health}/{Player.instance.maxHealth}";
        HealthSlider.value = Player.instance.health / (float)Player.instance.maxHealth;
    }

    private void ScoreChanged()
    {
        scoreText.text = $"Score: {Player.instance.score}";
    }

    private void WaveChanged()
    {
        waveText.text = $"Wave: {GameController.instance.currentWave}";
    }


    private void OnDisable()
    {
        Player.instance.bulletCountChanged.RemoveListener(BulletsChanged);
    }
}
