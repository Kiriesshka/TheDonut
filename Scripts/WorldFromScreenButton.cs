using UnityEngine;
using UnityEngine.Events;
using System.Collections;
public class WorldFromScreenButton : MonoBehaviour
{
    [SerializeField] private float timerBeforeAction;
    private Vector3 defaultLocalPosition;
    [SerializeField] Vector3 defaultOnEnableLocalPosition;
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
            transform.localPosition = Vector3.Lerp(transform.localPosition, defaultLocalPosition + defaultOnEnableLocalPosition, Time.deltaTime * 4);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, defaultLocalPosition, Time.deltaTime * 4);
        }
    }
    private void OnMouseDown()
    {
        if (isOn)
        {
            isOn = false;
            onDeactivate.Invoke();
            globalSettings.GetGameSound().MakeSound("Button", "World");
            return;
        }
        StartCoroutine(ActivationTimer());
        isOn = true;
    }
    private IEnumerator ActivationTimer()
    {
        yield return new WaitForSeconds(timerBeforeAction);
        onActivate.Invoke();
        globalSettings.GetGameSound().MakeSound("Button", "World");
    }
}
