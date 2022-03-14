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

    private void Start()
    {
        Player.instance.bulletCountChanged.AddListener(BulletsChanged);
        bulletsText.text = $"Bullets: {Player.instance.bullets}/{Player.instance.maxBullets}";

        Player.instance.healthChanged.AddListener(HealthChanged);
        HealthSlider.value = Player.instance.health / (float)Player.instance.maxHealth;

        Player.instance.scoreChanged.AddListener(ScoreChanged);
        scoreText.text = $"Score: {Player.instance.score}";

        GameController.instance.waveChanged.AddListener(WaveChanged);
        waveText.text = $"Wave: {GameController.instance.currentWave}";
    }

    private void BulletsChanged()
    {
        bulletsText.text = $"Bullets: {Player.instance.bullets}/{Player.instance.maxBullets}";
    }

    private void HealthChanged()
    {
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
