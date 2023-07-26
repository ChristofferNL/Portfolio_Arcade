using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This class takes a LevelDataSO and populates the preview canvas on the world map once the player has clicked on a stage
/// </summary>
public class LevelPreviewUIManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI stageNumber;
	[SerializeField] private string stageName;
	[SerializeField] private TextMeshProUGUI threeStarRequirement;
	[SerializeField] private TextMeshProUGUI twoStarRequirement;
	[SerializeField] private TextMeshProUGUI oneStarRequirement;
	[SerializeField] private TextMeshProUGUI currentHighscoreText;
	[SerializeField] private TextMeshProUGUI currentPersonalHighscoreText;
	[SerializeField] private Image previewImage;
	[SerializeField] private Button closeUIButton;
	[SerializeField] private Button startLevelButton;

	private int _highScore;

	private void Awake()
	{
		closeUIButton.onClick.AddListener(CloseUI);
		startLevelButton.onClick.AddListener(StartLevel);
		PersistentUserManager.Instance.AssignLevelPreviewUI(this);
		gameObject.SetActive(false);
	}

    public void AssignPreviewValues(LevelDataSO levelDataSO)
	{
		stageNumber.text = levelDataSO.StageNumber.ToString();
		stageName = levelDataSO.SceneName;
		threeStarRequirement.text = $"-> {levelDataSO.StarRequirements[2]}";
		twoStarRequirement.text = $"-> {levelDataSO.StarRequirements[1]}";
		oneStarRequirement.text = $"-> {levelDataSO.StarRequirements[0]}";
		currentPersonalHighscoreText.text = $"Your Best: {PlayerPrefs.GetInt(levelDataSO.StageNumber.ToString()).ToString()}";
		currentHighscoreText.text = "Waiting for server...";
		StartCoroutine(PersistentUserManager.Instance.ShowLevelHighscore());
	}

	public void UpdateHighscoreText(int newHighscore)
    {
		currentHighscoreText.text = $"Highscore: {newHighscore}";
	}

	public void StartLevel()
	{
		previewImage = null;
		PersistentUserManager.Instance.ChangeSceneEvent(stageName);
	}

	private void CloseUI()
	{
		this.gameObject.SetActive(false);
	}
}
