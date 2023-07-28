using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagerChess : MonoBehaviour
{
    public Button OptionsButton;
    public Button PlayButton;
    public Button QuitButton;
    public Button ExitOptionsButton;
    public Slider VolumeSlider;
    public Canvas OptionsMenu;
    public AudioSource MusicSource;
    public AudioSource EffectSource;
    public Toggle MusicToggle;
    public TextMeshProUGUI PressAnyKeyText;
    public AudioClip ButtonSound;

    private void Awake()
    {
        MusicSource = GetComponent<AudioSource>();
        OptionsMenu.gameObject.SetActive(false);
        ChangeVolume(VolumeSlider.value);
        OptionsButton.onClick.AddListener(ToggleOptionsMenu);
        ExitOptionsButton.onClick.AddListener(ToggleOptionsMenu);
        VolumeSlider.onValueChanged.AddListener(ChangeVolume);
        QuitButton.onClick.AddListener(QuitGame);
        MusicToggle.onValueChanged.AddListener(ToggleMusic);
        PlayButton.onClick.AddListener(PlayGame);
    }

	private void Start()
	{
		Cursor.visible = true;
	}

	private void Update()
    {
        if (Input.anyKeyDown && PressAnyKeyText.IsActive())
        {
            PressAnyKeyText.gameObject.SetActive(false);
            OptionsButton.gameObject.SetActive(true);
            PlayButton.gameObject.SetActive(true);
            QuitButton.gameObject.SetActive(true);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleSceneChess");
    }

    public void ToggleOptionsMenu()
    {
        EffectSource.clip = ButtonSound;
        EffectSource.Play();
        if (OptionsMenu.isActiveAndEnabled == true)
        {
            OptionsMenu.gameObject.SetActive(false);
        }
        else
        {
            OptionsMenu.gameObject.SetActive(true);
        }
    }

    public void ToggleMusic(bool isPlaying)
    {
        if (isPlaying)
        {
            MusicSource.UnPause();
        }
        else
        {
            MusicSource.Pause();
        }
    }

    public void ChangeVolume(float newVolume)
    {
        MusicSource.volume = newVolume / 3;
        EffectSource.volume = newVolume;
    }

    public void GameFinished()
    {
        OptionsButton.GetComponentInChildren<TextMeshProUGUI>().text = "Quit";
        OptionsButton.onClick.AddListener(QuitGame);
    }

    public void QuitGame()
    {
        EffectSource.clip = ButtonSound;
        EffectSource.Play();
        SceneManager.LoadScene(0);
        //UnityEditor.EditorApplication.isPlaying = false;
    }
}
