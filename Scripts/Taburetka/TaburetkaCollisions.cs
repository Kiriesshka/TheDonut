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

    [SerializeField] private int hpMax;
    public int hp;
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
        if (hp <= 0)
        {
            died.Invoke();
            Die();
        }
        else
        {
            hpChanged.Invoke();
        }
        if(enemyData.destroyAfterCollision)
            Destroy(enemyData.gameObject);

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
