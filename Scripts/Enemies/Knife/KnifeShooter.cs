using UnityEngine;

public class KnifeShooter : MonoBehaviour
{
    [SerializeField] private float timeToLaunchNext;
    [SerializeField] private GameObject knife;
    private float _timer;
    private void Update()
    {
        if(_timer >= 0)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            _timer = timeToLaunchNext;
            CreateNew();
        }
    }
    private void CreateNew()
    {
        GameObject k = Instantiate(knife, transform);
        k.transform.localPosition = new Vector3(0, 3.66f, -.2f);
        k.transform.SetParent(null);
        k.transform.localScale = Vector3.zero;
        k.AddComponent<FloatingKnife>().liveTime = 5;
        k.GetComponent<FloatingKnife>().movingDirection = -transform.forward;
        k.GetComponent<FloatingKnife>().speed = 5;
        k.GetComponent<FloatingKnife>().startDelay = 1;
    }

}
public class FloatingKnife : MonoBehaviour
{
    public Vector3 movingDirection;
    public float speed;
    public float liveTime;

    public float startDelay;
    private void Start()
    {
        Destroy(gameObject, liveTime);
    }
    private void Update()
    {
        if(startDelay > 0)
        {
            startDelay -= Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.5f, 1, 1f), Time.deltaTime * 10);
            return;
        }
        transform.Rotate(new Vector3(speed*-100 * Time.deltaTime, 0, 0));
        transform.position += movingDirection * Time.deltaTime*speed;
    }
}
