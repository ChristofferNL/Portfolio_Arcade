using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// SO containing the info needed for each level
/// </summary>
[CreateAssetMenu(menuName = "SO:s/New Level")]
public class LevelDataSO : ScriptableObject
{
    public int StageNumber;
    public int[] StarRequirements = new int[3];
    public AudioClip LevelMusic;
    public string SceneName;
}
