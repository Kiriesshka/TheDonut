using UnityEngine;

public class BaseRotator : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        transform.Rotate(new Vector3 (0, 1, 0), speed*Time.deltaTime);
    }
}
