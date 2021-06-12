using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] internal SettingsData settingsData; // Saving settings data across scenes using scriptable objects
    public static SettingsManager Instance { get; private set; } // This is a singleton object

    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _difficultySlider;
    [SerializeField] private Button _backButton;

    public event Action OnMusicVolumeChange;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    void Start()
    {
        Assert.IsNotNull( _musicSlider );
        Assert.IsNotNull( _sfxSlider );
        Assert.IsNotNull( _difficultySlider );
        Assert.IsNotNull( _backButton );

        _musicSlider.onValueChanged.AddListener(delegate{ UpdateMusicVolume(); });
        _sfxSlider.onValueChanged.AddListener(delegate{ UpdateSFXVolume(); });
        _difficultySlider.onValueChanged.AddListener(delegate{ UpdateGameSpeed(); });
        _backButton.onClick.AddListener(delegate{ ToggleSettingsPanel(); });

        _settingsPanel.SetActive(false);

        UIInvoker.OnOpenSettings += ToggleSettingsPanel;
    }

    void OnDestroy()
    {
        UIInvoker.OnOpenSettings -= ToggleSettingsPanel;
    }

    void OnEnable()
    {
        // Load data from player prefs
        settingsData.musicVolume = PlayerPrefs.HasKey("MusicVolume") ? PlayerPrefs.GetFloat("MusicVolume") : 100f; // If there's no save files for music volume defaults it to max
        settingsData.sfxVolume = PlayerPrefs.HasKey("SFXVolume") ? PlayerPrefs.GetFloat("SFXVolume") : 100f; // If there's no save files for sfx volume defaults it to max

        // Sets the current settings value to the value in the settings data
        _musicSlider.SetValueWithoutNotify(settingsData.musicVolume);
        _sfxSlider.SetValueWithoutNotify(settingsData.sfxVolume);
    }
    
    /// <summary>
    /// Toggles the settings panel on and off
    /// </summary>
    public void ToggleSettingsPanel()
    {
        AudioManager.Instance.Play("Select");
        _settingsPanel.SetActive(!_settingsPanel.activeSelf);
    }

    /// <summary>
    /// Updates the music volume data based on the current music volume
    /// </summary>
    private void UpdateMusicVolume()
    {
        //AudioManager.Instance.Play("");
        settingsData.musicVolume = _musicSlider.value;
        OnMusicVolumeChange?.Invoke();

        PlayerPrefs.SetFloat("MusicVolume", _musicSlider.value);
    }
    
    /// <summary>
    /// Updates the sfx volume data based on the current sfx volume
    /// </summary>
    private void UpdateSFXVolume()
    {
        //AudioManager.Instance.Play("");
        settingsData.sfxVolume = _sfxSlider.value;

        PlayerPrefs.SetFloat("SFXVolume", _sfxSlider.value);
    }

    /// <summary>
    /// Updates the game speed data based on the current game speed
    /// </summary>
    private void UpdateGameSpeed()
    {
        //AudioManager.Instance.Play("");

        PlayerPrefs.SetFloat("Difficulty", _difficultySlider.value);
    }
}
