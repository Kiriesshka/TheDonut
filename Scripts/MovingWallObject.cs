using UnityEngine;

public class MovingWallObject : MonoBehaviour
{
    [SerializeField] private Vector3[] beacons;
    [SerializeField] private float speed;
    private int currentBeacon;

    private bool stop = true;
    private void OnValidate()
    {
        for(int i = 0; i < beacons.Length-1; i++)
        {
            Debug.DrawLine(beacons[i], beacons[i + 1], Color.red);
        }
    }
    private void Update()
    {
        if (stop) return;
        Vector3 direction = (beacons[currentBeacon] - transform.position).normalized;
        transform.position += direction * Time.deltaTime * speed;
        if (Vector3.Distance(transform.position, beacons[currentBeacon]) <= 0.05f)
        {
            transform.position = beacons[currentBeacon];
            stop = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.name == "TheTaburetka")
        {
            collision.transform.SetParent(transform);
            if(stop)
                collision.gameObject.GetComponent<TaburetkaMovementController>().unableToGo = false;
            else
                collision.gameObject.GetComponent<TaburetkaMovementController>().unableToGo = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.IsChildOf(transform))
        {
            collision.transform.SetParent(null);
            collision.gameObject.GetComponent<TaburetkaMovementController>().unableToGo = false;
        }
    }
    private void MoveToNext()
    {
        currentBeacon += 1;
        if (currentBeacon >= beacons.Length) stop = true;
    }
    public void StartMove()
    {
        stop = false;
    }
    public void StartMoveTo(int beacon)
    {
        stop = false;
        currentBeacon = beacon;
    }
}
