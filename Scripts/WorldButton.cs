using UnityEngine;
using UnityEngine.Events;

public class WorldButton : MonoBehaviour, TaburetkaMovedHandler
{
    [SerializeField] float distanceToDeactivate = 1;
    [SerializeField] private Transform taburetka;
    [SerializeField] private bool noDeactivate;

    private Vector3 defaultLocalPosition;
    private bool isOn;

    public UnityEvent onActivate;
    public UnityEvent onDeactivate;

    private KT_GlobalSettings globalSettings;

    private void Start()
    {
        globalSettings = GameObject.Find("KT_GlobalSettings").GetComponent<KT_GlobalSettings>();

        defaultLocalPosition = transform.localPosition;
    }
    private void Update()
    {
        if (isOn)
        {
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
        globalSettings.GetGameSound().MakeSound("Button", "World");
        isOn = true;
    }
    public void HandleTaburetkaMovement(Transform t)
    {
        if (taburetka && t != taburetka)
        {
            if (Vector3.Distance(t.position, transform.position) > Vector3.Distance(taburetka.position, transform.position)) return;
        }
        taburetka = t;
        if (noDeactivate) return;
        if (!isOn) return;
        if (Vector3.Distance(transform.position, taburetka.position) >= distanceToDeactivate)
        {
            isOn = false;
            onDeactivate.Invoke();
            globalSettings.GetGameSound().MakeSound("Button", "World");
        }
    }
}
