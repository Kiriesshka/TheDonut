using UnityEngine;

public class HeartController : MonoBehaviour
{
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private TaburetkaCollisions taburetkaCollisions;
    private void Start()
    {
        UpdateHearts();
        taburetkaCollisions.hpChanged.AddListener(UpdateHearts);
    }
    private void UpdateHearts()
    {
        for(int i = 0; i < 3; i++)
        {
            if(i < taburetkaCollisions.hp)
                hearts[i].SetActive(true);
            else 
                hearts[i].SetActive(false);
        }
    }
}
