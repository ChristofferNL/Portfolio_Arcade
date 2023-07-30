using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles the starting screen UI elements and methods
/// </summary>
public class StartScreenUIManager : MonoBehaviour
{
    [SerializeField] private Canvas SettingsCanvas;
    [SerializeField] private Button SettingsButton;
    [SerializeField] private Button QuitButton;
    [SerializeField] private Button StartGameButton;

    private void Awake()
    {
        SettingsButton.onClick.AddListener(OpenSettingsMenu);
        StartGameButton.onClick.AddListener(StartGame);
        QuitButton.onClick.AddListener(QuitGame);
    }

    private void Start()
    {
        SettingsCanvas.gameObject.SetActive(false);

		Cursor.visible = true;
	}

    private void OpenSettingsMenu()
    {
        SettingsCanvas.gameObject.SetActive(true);
    }

    private void StartGame()
    {
        PersistentUserManager.Instance.ChangeSceneEvent("WorldMap");
    }

    private void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
}
