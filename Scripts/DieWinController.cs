using UnityEngine;
public class DieWinController : MonoBehaviour
{
    [SerializeField] private KT_Window loosedWindow;
    [SerializeField] private KT_Window winWindow;
    [SerializeField] private TaburetkaCollisions taburetkaCollisions;
    [SerializeField] private TextOutTypeA dieReasonText;
    private void Start()
    {
        taburetkaCollisions.died.AddListener(Loose);
        taburetkaCollisions.cakesBecomeMax.AddListener(Win);
    }
    private void Loose()
    {
        loosedWindow.Open();
        dieReasonText.PushText($"Причина смерти: {taburetkaCollisions.dieReason}");
    }
    private void Win()
    {
        winWindow.Open();
    }
}
