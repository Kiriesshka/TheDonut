using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public enum EnemyType
    {
        enemy,
        knife,
        Flamer
    }
    public EnemyType enemyType;
    public int damage = 1;
    public bool destroyAfterCollision;
}
