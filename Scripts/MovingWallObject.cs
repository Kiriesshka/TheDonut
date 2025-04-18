using UnityEngine;

public class MovingWallObject : MonoBehaviour
{
    [SerializeField] private Vector3[] beacons;
    [SerializeField] private float speed;
    [SerializeField] private int startMoveToStartIndex = -1;
    private int currentBeacon;

    private bool stop = true;

    private TaburetkaMovementController _taburetka;
    private void OnValidate()
    {
        for(int i = 0; i < beacons.Length-1; i++)
        {
            Debug.DrawLine(beacons[i], beacons[i + 1], Color.red);
        }
    }
    private void Start()
    {
        if(startMoveToStartIndex != -1)
        {
            StartMoveTo(startMoveToStartIndex);
        }
    }
    private void Update()
    {
        if (stop) return;
        Vector3 direction = (beacons[currentBeacon] - transform.localPosition).normalized;
        transform.localPosition += direction * Time.deltaTime * speed;
        if (Vector3.Distance(transform.localPosition, beacons[currentBeacon]) <= 0.05f)
        {
            transform.localPosition= beacons[currentBeacon];
            stop = true;
            if(_taburetka)
                _taburetka.UnFreeze();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_taburetka) return;

        if (collision.transform.name != "TheTaburetka") return;
        _taburetka = collision.gameObject.GetComponent<TaburetkaMovementController>();

        _taburetka.transform.SetParent(transform);
        if (!stop)
        {
            Debug.Log("FreezeFromCollisionEnter");
            _taburetka.Freeze();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (!stop) return; 
        if (collision.transform.IsChildOf(transform))
        {
            if (_taburetka)
            {
                _taburetka.transform.SetParent(null);
                Debug.Log("UnFreezeFromCollisionExit");
                _taburetka.UnFreeze();
                _taburetka = null;
            }
        }
    }
    private void MoveToNext()
    {
        currentBeacon += 1;
        if (currentBeacon >= beacons.Length) stop = true;
        if (_taburetka)
        {
            _taburetka.Freeze();
        }
    }
    public void StartMove()
    {
        stop = false;
    }
    public void StartMoveTo(int beacon)
    {
        stop = false;
        currentBeacon = beacon;
        if (_taburetka)
        {
            Debug.Log("FreezeFromStartMoveTo");
            _taburetka.Freeze();
        }
    }
}
