using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BildSupportManager : MonoBehaviour
{
	[SerializeField] GameObject tvOptionsScreen;
	[SerializeField] GameObject fruktOptionsScreen;
	[SerializeField] GameObject lekaOptionsScreen;

	[SerializeField] GameObject tvImagesScreen;
	[SerializeField] GameObject fruktImagesScreen;
	[SerializeField] GameObject lekaImagesScreen;

	[SerializeField] List<Toggle> tvToggles = new();
	[SerializeField] List<Toggle> fruktToggles = new();
	[SerializeField] List<Toggle> lekaToggles = new();

	[SerializeField] List<GameObject> tvImageObjects = new();
	[SerializeField] List<GameObject> fruktImageObjects = new();
	[SerializeField] List<GameObject> lekaImageObjects = new();

	private void Start()
	{
		BackToMainMenu();
	}

	public void BackToMainMenu()
	{
		tvOptionsScreen.SetActive(false);
		fruktOptionsScreen.SetActive(false);
		//lekaOptionsScreen.SetActive(false);
		tvImagesScreen.SetActive(false);
		fruktImagesScreen.SetActive(false);
		//lekaImagesScreen.SetActive(false);
	}

	public void OpenTVOptions()
	{
		tvOptionsScreen.SetActive(true);
	}

	public void ShowSelectedTVObjects()
	{
		tvImagesScreen.SetActive(true);
		for (int i = 0; i < tvToggles.Count; i++)
		{
			tvImageObjects[i].SetActive(tvToggles[i].isOn);
		}
	}

	public void OpenFruktOptions()
	{
		fruktOptionsScreen.SetActive(true);
	}

	public void ShowSelectedFruktObjects()
	{
		fruktImagesScreen.SetActive(true);
		for (int i = 0; i < fruktToggles.Count; i++)
		{
			fruktImageObjects[i].SetActive(fruktToggles[i].isOn);
		}
	}

	public void OpenLekaOptions()
	{

	}
}
