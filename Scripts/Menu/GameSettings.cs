using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameSettings : MonoBehaviour
{
    [SerializeField] private Slider scaleUISlider;
    [SerializeField] private KT_Window pushAgainWindow;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private CanvasScaler canvas;

    private float _currentUIScaleFactor = 0.5f;
    private bool _listenForAgain;
    private float _timer;

    private string _originalButtonText;

    [SerializeField] private bool isInLevel;
    private void Start()
    {
        LoadSettings();
        canvas.scaleFactor = _currentUIScaleFactor;

        if (isInLevel) return;
        _originalButtonText = buttonText.text;
    }

    private void Update()
    {
        if (isInLevel) return;
        if (_listenForAgain)
        {
            _timer -= Time.deltaTime;
            if(_timer <= 0)
            {
                canvas.scaleFactor = _currentUIScaleFactor;
                pushAgainWindow.Close();
                _listenForAgain = false;
                buttonText.text = _originalButtonText;
                scaleUISlider.interactable = true;
            }
        }
    }
    private void UpdateUIScale()
    {
        canvas.scaleFactor = scaleUISlider.value;
    }
    public void RequestUpdateUIScale()
    {
        if (_listenForAgain)
        {
            _listenForAgain = false;
            pushAgainWindow.Close();
            buttonText.text = _originalButtonText;
            _currentUIScaleFactor = canvas.scaleFactor;
            scaleUISlider.interactable = true;
            SaveGameSettings();
            return;
        }

        scaleUISlider.interactable = false;

        _timer = 5;
        UpdateUIScale();

        _listenForAgain = true;
        buttonText.text = "Ok";

        pushAgainWindow.Open();
    }
    public void ResetScaleSettings()
    {
        _currentUIScaleFactor = 0.5f;
        canvas.scaleFactor = _currentUIScaleFactor;
        scaleUISlider.value = _currentUIScaleFactor;

        SaveGameSettings();
    }
    private void SaveGameSettings()
    {
        PlayerPrefs.SetFloat("_currentUIScaleFactor", _currentUIScaleFactor);
        PlayerPrefs.Save();
    }
    private void LoadSettings()
    {
        _currentUIScaleFactor = PlayerPrefs.HasKey("_currentUIScaleFactor") ? PlayerPrefs.GetFloat("_currentUIScaleFactor") : _currentUIScaleFactor;
        if (isInLevel) return;
        scaleUISlider.value = _currentUIScaleFactor;
    }

}
