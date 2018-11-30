using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public ScoreManager scoreManager;
    public Text highScoreText;
    public Text totalPointsText;

    private void Start() {
        highScoreText.text = scoreManager.highScore.ToString();
        totalPointsText.text = scoreManager.totalPoints.ToString();
    }

    public void ThesaurusLink() {
        Application.OpenURL("https://www.dict.cc/");
    }

    public void Styrognome() {
        Application.OpenURL("http://www.styrognome.com");
    }
}
