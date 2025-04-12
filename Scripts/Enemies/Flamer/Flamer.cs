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
        if(_timer < 0)
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
        light.enabled = false;
        isActive = false;
    }
    private void Enable()
    {
        particleSystem.emissionRate = 23;
        light.enabled = true;
        isActive = true;

    }
}
