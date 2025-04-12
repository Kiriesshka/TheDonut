using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
[RequireComponent(typeof(Rigidbody))]
public class TaburetkaCollisions : MonoBehaviour
{
    [SerializeField] List<EnemyData.EnemyType> enemyTypes;

    private Rigidbody _rb;
    private bool _alreadyDied;
    public UnityEvent died;
    public UnityEvent hpChanged;
    public UnityEvent cakesChanged;
    public UnityEvent cakesBecomeMax;


    [SerializeField] private int hpMax;
    public int hp;
    [SerializeField] private int cakesMax;
    public int cakes;

    [SerializeField] private float timerResistance;
    private float _resistanceLeft;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (_resistanceLeft >= 0) _resistanceLeft -= Time.deltaTime;
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
                    if (cakes < cakesMax)
                    {
                        Destroy(other.gameObject.GetComponent<Collider>());
                        cakes++;
                        cakesChanged.Invoke();
                        if (cakes == cakesMax)
                        {
                            cakesBecomeMax.Invoke();
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
            Die();
        }
        if(enemyData.destroyAfterCollision)
            Destroy(enemyData.gameObject);

    }
    private void Win()
    {
        GetComponent<TaburetkaMovementController>().enabled = false;
        enabled = false;
    }
    private void Die()
    {
        GetComponent<TaburetkaMovementController>().enabled = false;
        _alreadyDied = true;
        died.Invoke();
        _rb.constraints = RigidbodyConstraints.None;
        _rb.useGravity = true;
        _rb.AddForceAtPosition(new Vector3(Random.Range(-1, 1f), 10, Random.Range(-1, 1)).normalized * 40, transform.position + new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), Random.Range(-1, 1)));
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
