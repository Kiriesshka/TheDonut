using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Hint : MonoBehaviour
{
    [SerializeField] private float distanceToShow = 2;
    [SerializeField] private Transform player;
    [SerializeField] private Image tintImage;
    [SerializeField] private TMP_Text text;

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if(distanceToPlayer < distanceToShow)
        {
            float colorAlpha = Mathf.Lerp(0, 0.8f, (distanceToShow - distanceToPlayer));
            tintImage.color = new Color(tintImage.color.r, tintImage.color.g, tintImage.color.b, colorAlpha);
            text.color = new Color(text.color.r, text.color.g, text.color.b, colorAlpha);
        }
        else
        {
            tintImage.color = new Color(tintImage.color.r, tintImage.color.g, tintImage.color.b, 0);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        }
        
    }
}
