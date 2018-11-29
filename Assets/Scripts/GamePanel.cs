using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class GamePanel : MonoBehaviour {
    public int maxLives = 3;
    public int timeLimit = 30;
    private int timeLeft;
    private int score;
    private int correct;
    public int lives;
    private bool gameEnded = false;

    private Question question;

    public Text timeLeftText;
    public Text wordText;
    public Text scoreText;

    public List<Image> lifeImages;
    public Sprite lifeImage;
    public Sprite missingLifeImage;

    public Transform answers;
    public Button answerPrefab;

    public GameEndPanel gameEndPanel;
    public GameObject timesUpPanel;
    public QuestionManager questionManager;
    public Thesaurus thesaurus;


    void Start() {
        InvokeRepeating("CheckTimer", 1, 1);
    }

    public void StartNewGame() {
        gameEnded = false;
        gameEndPanel.gameObject.SetActive(false);
        timeLeft = timeLimit;
        lives = maxLives;
        score = 0;
        scoreText.text = score.ToString();
        correct = 0;
        UpdateLifeImages();
        GetQuestion();
        gameObject.SetActive(true);
    }

    private void GetQuestion() {
        question = questionManager.GetQuestion();
        wordText.text = question.question;
        for (var i = 0; i < answers.childCount; i++) {
            Destroy(answers.GetChild(i).gameObject);
        }
        for (var j = 0; j < question.answers.Count; j++) {
            var button = Instantiate(answerPrefab, answers, false);
            var index = j;
            button.onClick.AddListener(() => Answer(index));
            button.GetComponentInChildren<Text>().text = question.answers[j];
        }
    }

    private void Answer(int index) {
        if (gameEnded) {
            return;
        }
        var buttonTransform = answers.GetChild(index);
        var button = buttonTransform.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        var buttonImage = buttonTransform.GetComponent<Image>();
        if (index == question.correct) {
            ShowTranslations();
            buttonImage.color = Color.green;
            correct++;
            score += 10;
            scoreText.text = score.ToString();
            StartCoroutine(GetQuestionAfterPause());
            return;
        }
        lives--;
        ShowTranslation(index);
        buttonImage.color = Color.red;
        UpdateLifeImages();
        if (lives == 0) {
            EndGame();
        }
    }

    private void ShowTranslations() {
        for (var i = 0; i < answers.childCount; i++) {
            ShowTranslation(i);
        }
    }

    private void ShowTranslation(int index) {
        var answerString = question.answers[index];
        var buttonTransform = answers.GetChild(index);
        var text = buttonTransform.GetComponentInChildren<Text>();
        var translation = thesaurus.GetRandomTranslationFr(answerString);
        text.text = answers + " - " + translation;
    }

    private IEnumerator GetQuestionAfterPause() {
        yield return new WaitForSeconds(0.5f);
        GetQuestion();
    }

    private void UpdateLifeImages() {
        for (var i = 0; i < maxLives; i++) {
            var image = lifeImages[i];
            image.sprite = i < lives ? lifeImage : missingLifeImage;
        }
    }

    private void CheckTimer() {
        if (timeLeft <= 0) {
            return;
        }
        timeLeft--;
        timeLeftText.text = timeLeft.ToString();

        if (timeLeft <= 0) {
            StartCoroutine(DisplayTimesUp());
            EndGame();
        }
    }

    private IEnumerator DisplayTimesUp() {
        timesUpPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        timesUpPanel.SetActive(false);
    }

    private void EndGame() {
        if (!gameEnded) {
            gameEnded = true;
            gameEndPanel.Show(lives, correct);
        }
    }
}
