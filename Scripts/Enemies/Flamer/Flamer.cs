using UnityEngine;

public class Flamer : EnemyData
{
    [SerializeField] private GameObject sparkPrefab;
    [SerializeField] private float timerActive;
    [SerializeField] private float timerSleep;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private Light light;

    public bool isActive;
    private float _timer;

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (isActive) light.intensity = Mathf.Lerp(light.intensity, 14, Time.deltaTime);
        else light.intensity = Mathf.Lerp(light.intensity, 0, Time.deltaTime*3);
        if (_timer < 0)
        {
            if (isActive)
            {
                Disable();
                _timer = timerSleep;
            }
            else
            {
                Enable();
                _timer = timerActive;
            }
        }
    }
    private void Disable()
    {
        particleSystem.emissionRate = 0;
        isActive = false;
    }
    private void Enable()
    {
        particleSystem.emissionRate = 23;
        isActive = true;

    }
}
