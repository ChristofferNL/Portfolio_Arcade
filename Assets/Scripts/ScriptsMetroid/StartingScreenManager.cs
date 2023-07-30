using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
/// <summary>
/// Creates the functionality needed for the startingscreen buttons
/// </summary>
public class StartingScreenManager : MonoBehaviour
{
    public Canvas StartScreen;
    public Canvas ControllerScreen;
    public Canvas CreditsScreen;

    private void Awake()
    {
        StartScreen.enabled = true;
        ControllerScreen.enabled = false;
        CreditsScreen.enabled = false;
	}

	private void Start()
	{
		Cursor.visible = true;
	}

	public void StartGame()
    {
        SceneManager.LoadScene("DefaultScene");
    }
    public void ControllersPage()
    {
        StartScreen.enabled = false;
        ControllerScreen.enabled = true;
    }
    public void CreditsPage()
    {
        StartScreen.enabled = false;
        CreditsScreen.enabled = true;
    }
    public void StartPage()
    {
        StartScreen.enabled = true;
        CreditsScreen.enabled = false;
        ControllerScreen.enabled = false;
    }
    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }
}
