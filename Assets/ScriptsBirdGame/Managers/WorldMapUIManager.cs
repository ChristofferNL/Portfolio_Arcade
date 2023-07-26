using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the settingsbutton and canvas in the world map scene
/// </summary>
public class WorldMapUIManager : MonoBehaviour
{
    [SerializeField] private Canvas SettingsCanvas;
    [SerializeField] private Button SettingsButton;


    private void Awake()
    {
        SettingsButton.onClick.AddListener(OpenSettingsMenu);
    }

    private void Start()
    {
        SettingsCanvas.gameObject.SetActive(false);
    }

    private void OpenSettingsMenu()
    {
        SettingsCanvas.gameObject.SetActive(true);
    }
}
