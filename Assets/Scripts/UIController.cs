using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public TMP_Text bulletsText;
    public Slider HealthSlider;

    private void Start()
    {
        Player.instance.bulletCountChanged.AddListener(BulletsChanged);
        bulletsText.text = $"Bullets: {Player.instance.bullets}/{Player.instance.maxBullets}";

        Player.instance.healthChanged.AddListener(HealthChanged);
        HealthSlider.value = Player.instance.health / (float)Player.instance.maxHealth;
    }

    private void BulletsChanged()
    {
        bulletsText.text = $"Bullets: {Player.instance.bullets}/{Player.instance.maxBullets}";
    }

    private void HealthChanged()
    {
        HealthSlider.value = Player.instance.health / (float)Player.instance.maxHealth;
    }


    private void OnDisable()
    {
        Player.instance.bulletCountChanged.RemoveListener(BulletsChanged);
    }
}
