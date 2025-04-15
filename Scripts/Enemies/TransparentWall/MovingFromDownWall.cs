using UnityEngine;

public class MovingFromDownWall : MonoBehaviour, TaburetkaMovedHandler
{
    [SerializeField] private Transform controlled;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float distanceToGoUp;
    [SerializeField] private Transform taburetka;
    [SerializeField] private float speed = 1;
    [SerializeField] private Vector3 offRotation;
    [SerializeField] private Vector3 onRotation;
    private bool isUp;
    private void Start()
    {
        if(offRotation == Vector3.zero)
        {
            offRotation = new Vector3(Random.Range(-90f, 90), Random.Range(-90f, 90), Random.Range(-90f, 90));
        }
    }
    private void Update()
    {
        if (isUp)
        {
            controlled.transform.position = Vector3.Lerp(controlled.transform.position, transform.position, Time.deltaTime * speed);
            controlled.transform.rotation = Quaternion.Lerp(controlled.transform.rotation, Quaternion.Euler(onRotation), Time.deltaTime * speed);
        }
        else
        {
            controlled.transform.position = Vector3.Lerp(controlled.transform.position, transform.position+offset, Time.deltaTime * speed);
            controlled.transform.rotation = Quaternion.Lerp(controlled.transform.rotation, Quaternion.Euler(offRotation), Time.deltaTime * speed);

        }
    }
    public void ReCalculate()
    {
        float distance = Vector3.Distance(taburetka.position, transform.position);
        if (distance < distanceToGoUp)
        {
            GoUp();
        }
        else
        {
            GoDown();
        }
    }
    private void GoUp()
    {
        isUp = true;
    }
    private void GoDown()
    {
        isUp = false;
    }
    public void HandleTaburetkaMovement()
    {
        ReCalculate();
    }
}
