using UnityEngine;
using UnityEngine.Events;

public class WorldButton : MonoBehaviour
{
    [SerializeField] float distanceToDeactivate = 1;
    [SerializeField] private Transform taburetka;

    private Vector3 defaultLocalPosition;
    private bool isOn;

    public UnityEvent onActivate;
    public UnityEvent onDeactivate;

    private void Start()
    {
        defaultLocalPosition = transform.localPosition;
    }
    private void Update()
    {
        if (isOn)
        {
            if (Vector3.Distance(transform.position, taburetka.position) >= distanceToDeactivate)
            {
                isOn = false;
                onDeactivate.Invoke();
            }
            transform.localPosition = Vector3.Lerp(transform.localPosition, defaultLocalPosition + new Vector3(0, -0.13f, 0), Time.deltaTime * 4);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, defaultLocalPosition, Time.deltaTime * 4);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (isOn) return;
        onActivate.Invoke();
        isOn = true;
    }
}
