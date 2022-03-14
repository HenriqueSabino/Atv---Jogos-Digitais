using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsSO", menuName = "Sciptable Object/Enemy Stats")]
public class EnemyStatsSO : ScriptableObject
{
    [Header("Stats")]
    public float speed;
    public float jumpVel;
    public int health;
    public int damage;

    [Header("Animation")]
    public RuntimeAnimatorController animController;
}