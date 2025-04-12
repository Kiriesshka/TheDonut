using UnityEngine;

public class DieWinController : MonoBehaviour
{
    [SerializeField] private KT_Window loosedWindow;
    [SerializeField] private TaburetkaCollisions taburetkaCollisions;
    private void Start()
    {
        taburetkaCollisions.died.AddListener(Loose);
    }
    private void Loose()
    {
        loosedWindow.Open();
    }
}
