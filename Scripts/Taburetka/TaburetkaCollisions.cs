using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
[RequireComponent(typeof(Rigidbody))]
public class TaburetkaCollisions : MonoBehaviour
{
    [SerializeField] List<EnemyData.EnemyType> enemyTypes;

    private Rigidbody _rb;
    private bool _alreadyDied;
    public string dieReason;
    public UnityEvent died;
    public UnityEvent hpChanged;
    public UnityEvent screwsChanged;
    public UnityEvent screwsBecomeMax;


    [SerializeField] private int hpMax;
    public int hp;
    [SerializeField] private int screwsMax;
    public int screws;

    [SerializeField] private float timerResistance;
    private float _resistanceLeft;

    [SerializeField] private float levelMinHeight = -5;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (_resistanceLeft >= 0) _resistanceLeft -= Time.deltaTime;
        //POSSIBLE BUG!!!
        if (transform.position.y <= levelMinHeight)
        {
            dieReason = "Вы выпали из уровня!";
            died.Invoke();
            Die();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (_alreadyDied) return;
        if (other.gameObject.TryGetComponent(out EnemyData enemyData))
        {
            if (enemyTypes.Contains(enemyData.enemyType))
            {
                if (enemyData.enemyType == EnemyData.EnemyType.Flamer)
                {
                    if (enemyData.GetComponent<Flamer>().isActive) OnEnemyCollision(enemyData);
                }
                else if(enemyData.enemyType == EnemyData.EnemyType.Cake && !other.GetComponent<MoveUpAndRotate>())
                {
                    if (screws < screwsMax)
                    {
                        Destroy(other.gameObject.GetComponent<Collider>());
                        screws++;
                        screwsChanged.Invoke();
                        if (screws == screwsMax)
                        {
                            screwsBecomeMax.Invoke();
                            Win();
                        }
                        other.gameObject.AddComponent<MoveUpAndRotate>();
                    }
                    
                }
                else
                {
                    OnEnemyCollision(enemyData);
                }
            }
        }
    }
    private void OnEnemyCollision(EnemyData enemyData)
    {
        if (_resistanceLeft > 0) return;
        _resistanceLeft = timerResistance;

        hp -= Mathf.Min(hp, enemyData.damage);
        hpChanged.Invoke();
        if (hp <= 0)
        {
            died.Invoke();
            dieReason = $"Вы погибли от [{enemyData.nameInDieReason}]";
            Die();
        }
        if(enemyData.destroyAfterCollision)
            Destroy(enemyData.gameObject);

    }
    private void Win()
    {
        GetComponent<MovementController>().enabled = false;
        enabled = false;
    }
    private void Die()
    {
        GetComponent<MovementController>().enabled = false;
        if(TryGetComponent(out MicrobroMovementController mmc))
        {
            Destroy(gameObject);
            return;
        }
        _alreadyDied = true;
        died.Invoke();
        _rb.constraints = RigidbodyConstraints.None;
        _rb.useGravity = true;
        _rb.AddForceAtPosition(new Vector3(Random.Range(-1, 1f), 10, Random.Range(-1, 1)).normalized * 100, transform.position + new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), Random.Range(-1, 1)));
        enabled = false;
    }
}
public class MoveUpAndRotate : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 3f);
    }
    private void Update()
    {
        transform.position += new Vector3(0, 3 * Time.deltaTime, 0);
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 3);
        transform.Rotate(new Vector3(0, Time.deltaTime * 100, 0));
    }
}
