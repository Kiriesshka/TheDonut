using UnityEngine;
using System.Collections;
public class MovementController:MonoBehaviour
{
    [SerializeField] protected float movementHandleRadius = 10;
}
[RequireComponent(typeof(Rigidbody))]
public class MicrobroMovementController : MovementController
{
    
    [SerializeField] private float speed;
    [SerializeField] private LayerMask wall;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private string[] sounds;

    private KT_GlobalSettings globalSettings;

    [SerializeField] Vector3[] turns;
    [SerializeField] private float timeSpaceBetweenTurns;

    private int _currentTurn;
    private float _time;
    private bool _isRolling;

    private Vector3 _startPosition;
    private void Start()
    {
        _startPosition = transform.position;
        _time = timeSpaceBetweenTurns;

        globalSettings = GameObject.Find("KT_GlobalSettings").GetComponent<KT_GlobalSettings>();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (!_isRolling)
        {
            _time -= Time.deltaTime;
            if (_time <= 0)
            {
                _time = timeSpaceBetweenTurns;
                if (CanRollInDirection(turns[_currentTurn]))
                {
                    StartCoroutine(Roll(turns[_currentTurn]));
                    NextTurn();
                }
            }
        }
    }
    public void ReturnToStartPosition()
    {
        _time = timeSpaceBetweenTurns;
        _currentTurn = 0;
        transform.position = _startPosition;
    }
    private void NextTurn()
    {
        _currentTurn++;
        if (_currentTurn >= turns.Length)
        {
            _currentTurn = 0;
        }
    }
    private void PreviousTurn()
    {
        _currentTurn--;
        if (_currentTurn < 0)
        {
            _currentTurn = turns.Length-1;
        }
    }
    private bool CanRollInDirection(Vector3 direction)
    {
        return !(Physics.Raycast(transform.position-new Vector3(0,0.21f), direction, 0.45f, wall)) && IsGrounded();
    }
    private bool IsGrounded()
    {
        bool isGr = Physics.Raycast(transform.position, Vector3.down, 0.28f, wall);
        return isGr;
    }
    IEnumerator Roll(Vector3 direction)
    {
        _isRolling = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        float remainingAngle = 90;
        Vector3 rotationCenter = (transform.position + direction / 4 + Vector3.down / 4);
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(speed * Time.deltaTime, remainingAngle);
            transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }
        if(sounds!=null && sounds.Length>0)
            globalSettings.GetGameSound().MakeSound(sounds[Random.Range(0, sounds.Length)], "World");

        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

        Collider[] colliders = Physics.OverlapSphere(transform.position, movementHandleRadius);

        foreach (var c in colliders)
        {
            if (c.TryGetComponent(out TaburetkaMovedHandler tmh))
            {
                tmh.HandleTaburetkaMovement(transform);
            }
        }

        _isRolling = false;
    }
}
