using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles all methods and data for the settings menu, storing the players settings using playerprefs
/// </summary>
public class SettingsUIManager : MonoBehaviour
{
    [SerializeField] private Sprite ToggleMusicOnSprite;
    [SerializeField] private Sprite ToggleMusicOffSprite;
    [SerializeField] private Button ExitSettingsButton;
    [SerializeField] private Button MusicToggle;
    [SerializeField] private Button RestartButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private Slider MusicVolumeSlider;
    [SerializeField] private Canvas SettingsCanvas;

    private bool isMusicEnabled;
    private float currentVolume;

    const string MUSICENABLED = "musicEnabled";
    const string VOLUME = "volume";

    private void Awake() //Initialize the settings
    {
        MusicToggle.onClick.AddListener(SwitchToggleImage);
        MusicVolumeSlider.onValueChanged.AddListener(SaveVolumePref);
        ExitSettingsButton.onClick.AddListener(ExitSettings);
        RestartButton.onClick.AddListener(RestartLevel);
        ExitButton.onClick.AddListener(ExitStage);
        if (PlayerPrefs.GetInt(MUSICENABLED) == 0)
        {
            isMusicEnabled = false;
            PersistentUserManager.Instance.ToggleMusic(false);
            MusicToggle.image.sprite = ToggleMusicOnSprite;
        }
        else
        {
            isMusicEnabled = true;
            PersistentUserManager.Instance.ToggleMusic(true);
            MusicToggle.image.sprite = ToggleMusicOffSprite;
        }
        currentVolume = PlayerPrefs.GetFloat(VOLUME);
        MusicVolumeSlider.value = currentVolume * 2;
        PersistentUserManager.Instance.ChangeSoundVolume(currentVolume);
        if (SceneManager.GetActiveScene().name == "StartScreen")
        {
            RestartButton.gameObject.SetActive(false);
            ExitButton.gameObject.SetActive(false);
        }
        if (SceneManager.GetActiveScene().name == "WorldMap")
        {
            RestartButton.gameObject.SetActive(false);
        }
    }

    private void ExitStage()
    {
        if (SceneManager.GetActiveScene().name == "WorldMap")
        {
            PersistentUserManager.Instance.ChangeSceneEvent("StartScreen");
        }
        else
        {
            PersistentUserManager.Instance.ChangeSceneEvent("WorldMap");
        }    
    }

    private void RestartLevel()
    {
        PersistentUserManager.Instance.ChangeSceneEvent(SceneManager.GetActiveScene().name);
    }

    private void ExitSettings()
    {
        SettingsCanvas.gameObject.SetActive(false);
    }

    private void SaveVolumePref(float value)
    {
        currentVolume = value / 2;
        PlayerPrefs.SetFloat(VOLUME, value / 2);
        PersistentUserManager.Instance.ChangeSoundVolume(value / 2);
    }

    private void SwitchToggleImage() // toggles the music visual setting and if it is playing or not
    {
        if (isMusicEnabled)
        {
            isMusicEnabled = false;
            MusicToggle.image.sprite = ToggleMusicOnSprite;
            PlayerPrefs.SetInt(MUSICENABLED, isMusicEnabled.GetHashCode());
            PersistentUserManager.Instance.ToggleMusic(false);
        }
        else
        {
            isMusicEnabled = true;
            MusicToggle.image.sprite = ToggleMusicOffSprite;
            PlayerPrefs.SetInt(MUSICENABLED, isMusicEnabled.GetHashCode());
            PersistentUserManager.Instance.ToggleMusic(true);
        }
    }
}
