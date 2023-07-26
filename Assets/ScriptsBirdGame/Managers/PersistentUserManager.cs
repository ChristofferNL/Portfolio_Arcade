using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton class that holds the current info of the level, music, sound effects and highscores, highscores are stored through a online leaderboard called dreamloleaderboard 
/// </summary>
public class PersistentUserManager : MonoBehaviour
{
	public static PersistentUserManager Instance;
	public UnityEvent<string> EventChangeScene;
	public LevelDataSO currentLevelDataSO;

	public AudioSource MusicSource;
	public AudioSource SFXSource;

	public LevelPreviewUIManager previewCanvas;

	private AudioClip _musicClip;
	private dreamloLeaderBoard _dreamloLeaderBoard;

	private int _currentHighscore;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Debug.Log("PersistentUserManager already exists");
		}
		
	}

	private void Start()
	{
		_dreamloLeaderBoard = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
	}

	/// <summary>
	/// DreamloLeaderBoards allow for free storing of 20 variables, i assign the score to dreamlo using the stagenumber as the name identifier for the leaderboard, this means
	/// there can only be one highscore per level but made the code much easier to figure out, and it seems to work quite well!
	/// </summary>
	/// <param name="newScore"></param>
	public void TryAssignNewHighscore(int newScore)
	{
		var scores = _dreamloLeaderBoard.ToListHighToLow();
		if (scores != null)
		{
			foreach (var score in scores)
			{
				if (score.playerName == currentLevelDataSO.StageNumber.ToString())
				{
					if (newScore > score.score)
					{
						_dreamloLeaderBoard.AddScore(currentLevelDataSO.StageNumber.ToString(), newScore, 0, "PlayerName");
						return;
					}
					else
					{
						return;
					}
				}
			}
			_dreamloLeaderBoard.AddScore(currentLevelDataSO.StageNumber.ToString(), newScore, 0, "PlayerName");
		} 
	}

	/// <summary>
	/// Game also stores your personal best score using PlayerPrefs
	/// </summary>
	/// <param name="newScore"></param>
	public void TryAssignNewPersonalHighscore(int newScore)
    {
		int currentHighscore = PlayerPrefs.GetInt(currentLevelDataSO.StageNumber.ToString());
        if (newScore > currentHighscore)
        {
			PlayerPrefs.SetInt(currentLevelDataSO.StageNumber.ToString(), newScore);
        }
    }

	private void UpdateHighscores()
	{

	}

	int CheckInt(string s)
	{
		int x = 0;

		int.TryParse(s, out x);
		return x;
	}

	/// <summary>
	/// This method gets the current highscore from the dreamlo server, i use IEnumerator to give the server a little time to return the data before proceeding
	/// </summary>
	/// <returns></returns>
	public IEnumerator ShowLevelHighscore()
	{
		_dreamloLeaderBoard.GetScores();

		yield return new WaitForSeconds(1);

		var scores = _dreamloLeaderBoard.ToListHighToLow();

		if (scores != null)
		{
			foreach (var score in scores)
			{
				if (int.Parse(score.playerName) == currentLevelDataSO.StageNumber)
				{
					if (SceneManager.GetActiveScene().name == "WorldMap")
					{
						previewCanvas.UpdateHighscoreText(score.score);
					}
					else
					{
						FindObjectOfType<GameManager>().UpdateHighscoreText(score.score);
					}
					yield break;
				}
			}
		}
		previewCanvas.UpdateHighscoreText(0);
		yield return null;
	}

	public void AssignLevelPreviewUI(LevelPreviewUIManager levelPreviewUIManager)
	{
		previewCanvas = levelPreviewUIManager;
	}

	public void LoadLevelUI(LevelDataSO levelDataSO)
	{
		currentLevelDataSO = levelDataSO;
		previewCanvas.gameObject.SetActive(true);
		previewCanvas.AssignPreviewValues(levelDataSO);
	}

	public void SetCurrentMusic(AudioClip audioClip)
	{
		_musicClip = audioClip;
	}

	public void ChangeSoundVolume(float value)
	{
		MusicSource.volume = value;
		SFXSource.volume = value;
	}

	public void ToggleMusic(bool isActive)
	{
		if (isActive)
		{
			if (MusicSource.isPlaying)
			{
				MusicSource.Pause();
			}
		}
		else
		{
			if (!MusicSource.isPlaying)
			{
				MusicSource.Play();
			}         
		}
	}

	public void ChangeSceneEvent(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
}
