using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameEndPanel : MonoBehaviour {
    public ScoreManager scoreManager;

    public Text livesText;
    public Text livesPointsText;
    public Text correctText;
    public Text correctPointsText;
    public Text totalScoreText;
    public Text highScoreText;
    public Text totalPointsText;
    public GameObject newHighScore;

    public void Show(int lives, int correct) {
        var livesPoints = lives * ScoreManager.pointsPerLife;
        var correctPoints = correct * ScoreManager.pointsPerCorrect;
        var score = livesPoints + correctPoints;
        livesText.text = lives.ToString();
        livesPointsText.text = livesPoints.ToString();
        correctText.text = correct.ToString();
        correctPointsText.text = correctPoints.ToString();
        totalScoreText.text = score.ToString();

        var isHighScore = scoreManager.RegisterScore(score);

        newHighScore.SetActive(isHighScore);
        highScoreText.text = scoreManager.highScore.ToString();
        totalPointsText.text = scoreManager.totalPoints.ToString();

        gameObject.SetActive(true);
    }
}
