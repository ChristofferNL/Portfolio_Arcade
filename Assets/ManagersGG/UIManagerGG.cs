using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManagerGG : MonoBehaviour
{
    public static UIManagerGG Instance;
    public Button ChangeGolemButton;
    public Button ChangeEquipmentButton;
    public Button JumpButton;
    public Button SmashButton;
    public Button ExitGameButton;
    public TextMeshProUGUI DestroyedBuildingsText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        JumpButton.onClick.AddListener(ExecuteJump);
        ExitGameButton.onClick.AddListener(ExitApplication);
    }

	private void Start()
	{
		Cursor.visible = true;
	}

	public void SetDestroyedBuildingsText(int destroyedBuildings)
    {
        DestroyedBuildingsText.text = $"Buildings Destroyed: {destroyedBuildings.ToString()}";
    }

    public void ExecuteJump()
    {
        EventManagerGG.Instance.JumpInputEvent(true);
    }

    public void ExitApplication()
    {
        SaveManager.Instance.SaveData();
        Debug.Log("Exit Game");
        SceneManager.LoadScene(0);
    }
}
