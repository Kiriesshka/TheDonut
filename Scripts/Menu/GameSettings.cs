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

    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundGlobalVolumeSlider;
    [SerializeField] private Slider worldEffectsSlider;
    [SerializeField] private Slider uiEffectsSlider;

    private float _musicVolume = 0.5f;
    private float _soundGlobalVolume = 1f;
    private float _worldEffectsVolume = 1f;
    private float _uiEffectsVolume = 1f;

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
    private void ApplySoundSettings()
    {
        KT_GameSound k = GetComponent<KT_GameSound>();
        k.channelsVolumeSettings[2] = _musicVolume * _soundGlobalVolume;
        k.channelsVolumeSettings[1] = _worldEffectsVolume * _soundGlobalVolume;
        k.channelsVolumeSettings[0] = _uiEffectsVolume * _soundGlobalVolume;

        k.ApplySoundSettingsForMusic();

        SaveGameSettings();
    }

    public void SetMusicVolume()
    {
        _musicVolume = musicVolumeSlider.value;
        PlayerPrefs.SetFloat("_musicVolume", _musicVolume);
        ApplySoundSettings();
    }
    public void SetSoundGlobalVolume()
    {
        _soundGlobalVolume = soundGlobalVolumeSlider.value;
        PlayerPrefs.SetFloat("_soundGlobalVolume", _soundGlobalVolume);
        ApplySoundSettings();
    }
    public void SetWorldEffectsVolume()
    {
        _worldEffectsVolume = worldEffectsSlider.value;
        PlayerPrefs.SetFloat("_worldEffectsVolume", _worldEffectsVolume);
        ApplySoundSettings();
    }
    public void SetUiEffetcsVolume()
    {
        _uiEffectsVolume = uiEffectsSlider.value;
        PlayerPrefs.SetFloat("_uiEffectsVolume", _uiEffectsVolume);
        ApplySoundSettings();
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
        _soundGlobalVolume = PlayerPrefs.HasKey("_soundGlobalVolume") ? PlayerPrefs.GetFloat("_soundGlobalVolume") : _soundGlobalVolume;
        _musicVolume = PlayerPrefs.HasKey("_musicVolume") ? PlayerPrefs.GetFloat("_musicVolume") : _musicVolume;
        _uiEffectsVolume = PlayerPrefs.HasKey("_uiEffectsVolume") ? PlayerPrefs.GetFloat("_uiEffectsVolume") : _uiEffectsVolume;
        _worldEffectsVolume = PlayerPrefs.HasKey("_worldEffectsVolume") ? PlayerPrefs.GetFloat("_worldEffectsVolume") : _worldEffectsVolume;

        ApplySoundSettings();


        if (isInLevel) return;
        scaleUISlider.value = _currentUIScaleFactor;
        musicVolumeSlider.value = _musicVolume;
        soundGlobalVolumeSlider.value = _soundGlobalVolume;
        uiEffectsSlider.value = _uiEffectsVolume;
        worldEffectsSlider.value = _worldEffectsVolume;

    }

}
