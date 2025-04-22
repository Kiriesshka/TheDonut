using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Hint : MonoBehaviour, TaburetkaMovedHandler
{
    [SerializeField] private float distanceToShow = 2;
    [SerializeField] private Transform player;
    [SerializeField] private Image tintImage;
    [SerializeField] private TMP_Text text;
    [SerializeField] private bool enableOnStart;

    private bool _disabled = true;
    private void Start()
    {
        if (enableOnStart)
        {
            _disabled = false;
            return;
        }
        _disabled = true;
        tintImage.color = new Color(tintImage.color.r, tintImage.color.g, tintImage.color.b, 0);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }
    private void Update()
    {
        if (_disabled) return;
        
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        float colorAlpha = Mathf.Lerp(0, 0.8f, (distanceToShow - distanceToPlayer));
        tintImage.color = new Color(tintImage.color.r, tintImage.color.g, tintImage.color.b, colorAlpha);
        text.color = new Color(text.color.r, text.color.g, text.color.b, colorAlpha);
    }
    public void HandleTaburetkaMovement(Transform t)
    {
        player = t;
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        _disabled = distanceToPlayer > distanceToShow;
        if (_disabled)
        {
            tintImage.color = new Color(tintImage.color.r, tintImage.color.g, tintImage.color.b, 0);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        }
}
}
