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
    public string soundOnTriggerOnWall;
    public string soundOnTriggerOnTaburetkaHitted;

    private KT_GameSound gameSound;
    private void Start()
    {
        gameSound = GameObject.Find("KT_GlobalSettings").GetComponent<KT_GameSound>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.name == "Wall")
        {
            Destroy(gameObject);
            if(soundOnTriggerOnWall != "")
                gameSound.MakeSound(soundOnTriggerOnWall, "World");
        }
    }
    public void MakeHitTaburetkaSound()
    {
        if (soundOnTriggerOnTaburetkaHitted != "")
            gameSound.MakeSound(soundOnTriggerOnTaburetkaHitted, "World");
    }
}
