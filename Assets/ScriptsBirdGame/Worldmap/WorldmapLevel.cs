using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is applied to every stage/button on the world map, and is then assigned the LevelDataSO 
/// </summary>
public class WorldmapLevel : MonoBehaviour
{
    [SerializeField] private LevelDataSO levelData;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ShowLevelPreview);
    }

    public void ShowLevelPreview()
    {
        Debug.Log("showing level UI");
        PersistentUserManager.Instance.LoadLevelUI(levelData);
    }

}
