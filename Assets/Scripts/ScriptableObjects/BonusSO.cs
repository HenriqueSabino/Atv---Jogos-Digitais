using UnityEngine;

[CreateAssetMenu(fileName = "BonusSO", menuName = "Scriptable Object/Bonus")]
public class BonusSO : ScriptableObject
{
    public Sprite sprite;

    public int maxHealth;
    public int health;
    public int maxBullets;
    public int damage;

    public bool specialBonus;
    public string specialBonusTag;
}