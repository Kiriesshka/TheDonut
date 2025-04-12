using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public enum EnemyType
    {
        Enemy,
        Cake,
        Knife,
        Flamer,
        Hammer
    }
    public EnemyType enemyType;
    public int damage = 1;
    public bool destroyAfterCollision;
}
