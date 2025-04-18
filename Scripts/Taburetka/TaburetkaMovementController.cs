using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class TaburetkaMovementController : MovementController
{
    [SerializeField] private float speed;
    [SerializeField] private Transform donut;
    [SerializeField] private LayerMask wall;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private string[] sounds;

    [SerializeField] private KT_HoldableButton w;
    [SerializeField] private KT_HoldableButton a;
    [SerializeField] private KT_HoldableButton s;
    [SerializeField] private KT_HoldableButton d;

    private KT_GlobalSettings globalSettings;

    private bool _isFreezed = false;

    private bool isRolling = false;

    private void Start()
    {
        globalSettings = GameObject.Find("KT_GlobalSettings").GetComponent<KT_GlobalSettings>();
        isRolling = false;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (isRolling || _isFreezed) return;
        if ((Input.GetKey(KeyCode.S) || s.isOn) && CanRollInDirection(Vector3.back))
        {
            StartCoroutine(Roll(Vector3.back));
        }
        else if ((Input.GetKey(KeyCode.W) || w.isOn) && CanRollInDirection(Vector3.forward))
        {
            StartCoroutine(Roll(Vector3.forward));
        }
        else if ((Input.GetKey(KeyCode.A) || a.isOn) && CanRollInDirection(Vector3.left))
        {
            StartCoroutine(Roll(Vector3.left));
        }
        else if ((Input.GetKey(KeyCode.D) || d.isOn) && CanRollInDirection(Vector3.right))
        {
            StartCoroutine(Roll(Vector3.right));
        }

    }
    private bool CanRollInDirection(Vector3 direction)
    {
        return !(Physics.Raycast(transform.position + direction / 2f, direction, 0.55f, wall))&&IsGrounded();
    }
    private bool IsGrounded()
    {
        bool isGr = Physics.Raycast(transform.position, Vector3.down, 0.55f, wall);
        return isGr;
    }
    IEnumerator Roll(Vector3 direction)
    {
        isRolling = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        float remainingAngle = 90;
        Vector3 rotationCenter = transform.position + direction / 2 + Vector3.down / 2;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

        while(remainingAngle > 0)
        {
            float rotationAngle =Mathf.Min(speed * Time.deltaTime, remainingAngle);
            transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }
        if (sounds != null && sounds.Length > 0)
            globalSettings.GetGameSound().MakeSound(sounds[Random.Range(0, sounds.Length)], "World");
        isRolling = false;
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

        Collider[] colliders = Physics.OverlapSphere(transform.position, movementHandleRadius);

        foreach(var c in colliders)
        {
            if(c.TryGetComponent(out TaburetkaMovedHandler tmh))
            {
                tmh.HandleTaburetkaMovement(transform);
            }
        }
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
    }
    public void Freeze()
    {
        if (_isFreezed) return;
        _isFreezed = true;
        isRolling = false;
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
        transform.eulerAngles = new Vector3(Mathf.RoundToInt(transform.eulerAngles.x), Mathf.RoundToInt(transform.eulerAngles.y), Mathf.RoundToInt(transform.eulerAngles.z));
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    public void UnFreeze()
    {
        if (!_isFreezed) return;
        _isFreezed = false;
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
        transform.eulerAngles = new Vector3(Mathf.RoundToInt(transform.eulerAngles.x), Mathf.RoundToInt(transform.eulerAngles.y), Mathf.RoundToInt(transform.eulerAngles.z));
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }
}
