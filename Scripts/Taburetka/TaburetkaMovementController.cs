using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class TaburetkaMovementController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform donut;
    [SerializeField] private LayerMask wall;
    [SerializeField] private Rigidbody rb;

    private bool isRolling = false;
    private void Start()
    {
        isRolling = false;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (isRolling) return;
        if (Input.GetKey(KeyCode.S) && CanRollInDirection(Vector3.back))
        {
            StartCoroutine(Roll(Vector3.back));
        }
        else if (Input.GetKey(KeyCode.W) && CanRollInDirection(Vector3.forward))
        {
            StartCoroutine(Roll(Vector3.forward));
        }
        else if (Input.GetKey(KeyCode.A) && CanRollInDirection(Vector3.left))
        {
            StartCoroutine(Roll(Vector3.left));
        }
        else if (Input.GetKey(KeyCode.D) && CanRollInDirection(Vector3.right))
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
        isRolling = false;
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }
}
