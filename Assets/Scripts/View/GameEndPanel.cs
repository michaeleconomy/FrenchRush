using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.iOS;

public class GameEndPanel : MonoBehaviour {
    public ScoreManager scoreManager;

    public int promptLimit = 200;
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

        PromptReview();
    }

    private void PromptReview() {
        var hasPrompted = PlayerPrefs.HasKey("promptReview");
        if (!hasPrompted && scoreManager.totalPoints >= promptLimit) {
            PlayerPrefs.SetString("promptReview", "true");
            UIDialog.Display("Are you enjoying French Rush?", new string[] { "Yes", "No" }, false, null, (option, _) => {
                if (option == 0) {
                    if (!Device.RequestStoreReview()) {
                        UIDialog.Alert("Glad to hear it!");
                    }
                }
                else {
                    SendEmail();
                }
            });
        }
    }

    void SendEmail() {
        var email = "michaeleconomy" +
            '@' +
            "gmail.com";
        var subject = MyEscapeURL("French Rush Feedback");
        Application.OpenURL("mailto:" + email + "?subject=" + subject);
    }

    string MyEscapeURL(string url) {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }
}
