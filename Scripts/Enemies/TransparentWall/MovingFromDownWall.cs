using UnityEngine;

public class MovingFromDownWall : TaburetkaMovedHandler
{
    [SerializeField] private Transform controlled;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float distanceToGoUp;
    [SerializeField] private Transform taburetka;
    private bool isUp;
    private void Update()
    {
        if (isUp)
        {
            controlled.transform.position = Vector3.Lerp(controlled.transform.position, transform.position, Time.deltaTime * 5);
        }
        else
        {
            controlled.transform.position = Vector3.Lerp(controlled.transform.position, transform.position+offset, Time.deltaTime * 5);
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
    public override void HandleTaburetkaMovement()
    {
        ReCalculate();
    }
}
