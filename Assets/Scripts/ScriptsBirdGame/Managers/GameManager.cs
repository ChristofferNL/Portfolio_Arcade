using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] private int amountOfItems;
    [SerializeField] private TextMeshProUGUI collectedItemsText;
    [SerializeField] private TextMeshProUGUI timePassedText;
    [SerializeField] private Canvas levelCompleteCanvas;
    [SerializeField] private Canvas SettingsCanvas;
    [SerializeField] private Button SettingsButton;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI stageCompletedText;
    [SerializeField] private Button RestartButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private Button HighScoreButton;
    [SerializeField] private Sprite EmptyStarSprite;
    [SerializeField] private Sprite FullStarSprite;
    [SerializeField] private Image[] StarImages;
    private NestScript nest;
    private PlayerCollisionBirdGame playerCollision;
    private int amountOfItemsPickedUp;
    private bool gameIsRunning;
    private bool isShowingHighScore;
    private float timePassed;
    private float score;
    [SerializeField] private LevelDataSO currentLevelSO;

    const string HIGHSCORE = "highscore";

    private void Awake()
    {
        PickUpItem[] pickUps = FindObjectsOfType<PickUpItem>();
        amountOfItems = pickUps.Length;
        EventManager.Instance.EventItemPickUp.AddListener(AddPickUpItem);
        SetCollectedItemsUI();
        EventManager.Instance.EventFlapInput.AddListener(StartGame);
        EventManager.Instance.EventMoveInput.AddListener(StartGame);
        nest = FindObjectOfType<NestScript>();
        playerCollision = FindObjectOfType<PlayerCollisionBirdGame>();
        float minutes = Mathf.FloorToInt(timePassed / 60);
        float seconds = Mathf.FloorToInt(timePassed % 60);
        timePassedText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        RestartButton.onClick.AddListener(RestartLevel);
        HighScoreButton.onClick.AddListener(ShowHighScore);
        ExitButton.onClick.AddListener(ExitGame);
        SettingsButton.onClick.AddListener(ToggleSettingsCanvas);
        levelCompleteCanvas.gameObject.SetActive(false);
        SettingsCanvas.gameObject.SetActive(false);
    }

   

    private void Update()
    {
        TimerController();
    }

    public void TimerController()
    {
        if (gameIsRunning)
        {
            timePassed += Time.deltaTime;
            float minutes = Mathf.FloorToInt(timePassed / 60);
            float seconds = Mathf.FloorToInt(timePassed % 60);
            timePassedText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }   
    }

    private void ToggleSettingsCanvas()
    {
        if (SettingsCanvas.isActiveAndEnabled)
        {
            SettingsCanvas.gameObject.SetActive(false);
        }
        else
        {
            SettingsCanvas.gameObject.SetActive(true);
        }
    }

    private void SetStarAmount(int newScore)
    {
        for (int i = 0; i < StarImages.Length; i++)
        {
            if (newScore >= currentLevelSO.StarRequirements[i])
            {
                StarImages[i].sprite = FullStarSprite;
            }
            else
            {
                StarImages[i].sprite = EmptyStarSprite;
            }
        }
    }

    private void ShowHighScore()
    {
        if (isShowingHighScore)
        {
            finalScoreText.text = $"Score: {(int)score}";
            isShowingHighScore = false;
        }
        else
        {
            finalScoreText.text = $"Waiting for server...";
            StartCoroutine(PersistentUserManager.Instance.ShowLevelHighscore());
            isShowingHighScore = true;
        }
    }

    public void UpdateHighscoreText(int newScore)
    {
        finalScoreText.text = $"High Score: {newScore}";
    }

    private void CheckHighScore(float newScore)
    {
        PersistentUserManager.Instance.TryAssignNewHighscore((int)newScore);
        PersistentUserManager.Instance.TryAssignNewPersonalHighscore((int)newScore);
    }

    private void ExitGame()
    {
        PersistentUserManager.Instance.ChangeSceneEvent("WorldMap");
    }

    private void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private void StartGame(bool isPressed)
    {
        if (isPressed)
        {
			gameIsRunning = true;
			EventManager.Instance.EventFlapInput.RemoveListener(StartGame);
			EventManager.Instance.EventMoveInput.RemoveListener(StartGame);
		}	
    }

    private void StartGame(float input)
	{
        if (input != 0)
        {
			gameIsRunning = true;
			EventManager.Instance.EventFlapInput.RemoveListener(StartGame);
			EventManager.Instance.EventMoveInput.RemoveListener(StartGame);
		}        
	}

    private void EndRound()
    {
        score = CalculateFinalScore();
        CheckHighScore(score);
        finalScoreText.text = $"Score: {(int)score}";
        levelCompleteCanvas.gameObject.SetActive(true);
        SetStarAmount((int)score);
        stageCompletedText.text = $"Stage {PersistentUserManager.Instance.currentLevelDataSO.StageNumber} Completed";
        EventManager.Instance.EventFinishRound.RemoveListener(EndRound);
        gameIsRunning = false;
        GameObject controller = FindObjectOfType<PlayerController>().gameObject;
        if (controller != null)
        {
            controller.GetComponent<Rigidbody>().isKinematic = true;
        }

    }

    private float CalculateFinalScore()
    {
        float timeScore = (200 - timePassed) * 100;
        float penaltyScore = (playerCollision.GetRuffledFeatherValue() * 1500);
        float finalScore = timeScore - penaltyScore;
        if (finalScore < 0)
        {
            finalScore = 0;
        }
        return finalScore;
    }

	private void AddPickUpItem()
    {
        amountOfItemsPickedUp++;
        if (amountOfItemsPickedUp == amountOfItems)
        {
            EventManager.Instance.EventFinishRound.AddListener(EndRound);
        }
        SetCollectedItemsUI();
    }

    private void SetCollectedItemsUI()
    {
        collectedItemsText.text = $"{amountOfItemsPickedUp} / {amountOfItems}";
    } 
}
