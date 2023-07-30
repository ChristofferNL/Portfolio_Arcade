using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TetrisScoreEngine : MonoBehaviour
{
    public TetrisEngine engine;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI linesText;
    public int score;
    public int level;
    public int newLevel;
    public int lines;

    private void Start()
    {
        engine = this.GetComponent<TetrisEngine>();
        score = 0;
        level = 1;
        lines = 0;
        UpdateUIText();
    }

    public void CalculateScore(int linesAmount)
    {
        if (linesAmount == 1)
        {
            score += 40 * level;
        }
        if (linesAmount == 2)
        {
            score += 100 * level;
        }
        if (linesAmount == 3)
        {
            score += 300 * level;
        }
        if (linesAmount == 4)
        {
            score += 1200 * level;
        }
        lines += linesAmount;
        newLevel += linesAmount;
        if (newLevel > 9)
        {
            level++;
            newLevel = newLevel -10;
            engine.ChangeGameSpeed();
            UpdateUIText();
        }
        UpdateUIText();
    }
    private void UpdateUIText()
    {
        scoreText.text = $"Score: {score}";
        levelText.text = $"Level: {level}";
        linesText.text = $"Lines: {lines}";
    }
}
