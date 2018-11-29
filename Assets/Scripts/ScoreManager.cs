using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {
    public int highScore;
    public int totalPoints;
    public const int pointsPerLife = 30;
    public const int pointsPerCorrect = 10;


    private void Awake() {
        highScore = PlayerPrefs.GetInt("highScore", 0);
        totalPoints = PlayerPrefs.GetInt("totalPoints", 0);
    }

    public bool RegisterScore(int score) {
        var newHigh = false;
        if (score > highScore) {
            highScore = score;
            PlayerPrefs.SetInt("highScore", highScore);
            newHigh = true;
        }

        totalPoints += score;
        PlayerPrefs.SetInt("totalPoints", totalPoints);

        return newHigh;
    }
}
