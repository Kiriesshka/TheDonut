using UnityEngine;

public class ConstantRotator : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private float angle;

    void Update()
    {
        transform.Rotate(direction, angle * Time.deltaTime);
    }
}
