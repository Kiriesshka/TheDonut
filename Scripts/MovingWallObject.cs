using UnityEngine;

public class MovingWallObject : MonoBehaviour
{
    [SerializeField] private Vector3[] beacons;
    [SerializeField] private float speed;
    [SerializeField] private int startMoveToStartIndex = -1;
    private int currentBeacon;


    [SerializeField] private Vector3[] rotationBeacons;
    [SerializeField] private float rotate_speed;
    private int currentRotateBeacon;
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
        if(transform.localPosition!= beacons[currentBeacon])
        {
            Vector3 direction = (beacons[currentBeacon] - transform.localPosition).normalized;
            transform.localPosition += direction * Time.deltaTime * speed;
            if (Vector3.Distance(transform.localPosition, beacons[currentBeacon]) <= 0.05f)
            {
                transform.localPosition = beacons[currentBeacon];
                stop = true;
                if (_taburetka)
                    _taburetka.UnFreeze();
            }
        }
        if (transform.rotation != Quaternion.Euler(rotationBeacons[currentRotateBeacon]))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotationBeacons[currentRotateBeacon]), Time.deltaTime*rotate_speed);
            if(Quaternion.Angle(transform.rotation, Quaternion.Euler(rotationBeacons[currentRotateBeacon])) < 0.5f)
            {
                transform.rotation = Quaternion.Euler(rotationBeacons[currentRotateBeacon]);
                stop = true;
                if (_taburetka)
                    _taburetka.UnFreeze();
            }
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
    public void StartRotateTo(int beacon)
    {
        stop = false;
        currentRotateBeacon = beacon;
        if (_taburetka)
        {
            _taburetka.Freeze();
        }
    }
    public void StartMoveTo(int beacon)
    {
        currentBeacon = beacon;
        if (transform.localPosition != beacons[currentBeacon])
        {
            stop = false;
            if (_taburetka)
            {
                _taburetka.Freeze();
                Debug.Log("FREEEZED!");
            }
        }
    }
}
