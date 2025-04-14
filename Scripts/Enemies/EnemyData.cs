using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public enum EnemyType
    {
        Enemy,
        Cake,
        Knife,
        Flamer,
        Hammer,
        Button
    }
    public EnemyType enemyType;
    public int damage = 1;
    public bool destroyAfterCollision;
    public string nameInDieReason;
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.name == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
