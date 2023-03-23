using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startUI;
    [SerializeField]
    private GameObject scoreUI;
    [SerializeField]
    private GameObject gameEndedUI;

    [SerializeField]
    private TMPro.TextMeshProUGUI score;
    [SerializeField]
    private TMPro.TextMeshProUGUI playerLives;
    [SerializeField]
    private TMPro.TextMeshProUGUI currentLevel;
    [SerializeField]
    private TMPro.TextMeshProUGUI gameEndedScore;


    public void UpdateScore(int scoreValue)
    {
        score.text = string.Format(Constants.Score, scoreValue);
    }

    public void UpdateLives(int remainingLives)
    {
        playerLives.text = string.Format(Constants.Remaining_Lives, remainingLives);
    }

    public void ShowPlayUI(bool value)
    {
        startUI.SetActive(!value);
        gameEndedUI.SetActive(!value);
        scoreUI.SetActive(value);
    }

    public void GameEnded(int score)
    {
        startUI.SetActive(false);
        scoreUI.SetActive(false);
        gameEndedUI.SetActive(true);
        gameEndedScore.text = string.Format(Constants.Total_Score, score);
    }

    public void UpdateLevels(int level)
    {
        currentLevel.text = string.Format(Constants.Current_Level, level);
    }
}
