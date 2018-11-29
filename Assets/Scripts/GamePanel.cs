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
    private bool gameEnded = true;

    private Question question;

    public Text timeLeftText;
    public Text wordText;
    public Text scoreText;

    public List<Image> lifeImages;
    public Sprite lifeImage;
    public Sprite missingLifeImage;

    public Transform answers;
    public Button answerPrefab;

    public MainMenu mainMenu;
    public GameEndPanel gameEndPanel;
    public QuestionManager questionManager;
    public Thesaurus thesaurus;

    void Start() {
        InvokeRepeating("CheckTimer", 1, 1);
    }

    public void StartNewGame() {
        if (!gameEnded) {
            return;
        }
        gameEnded = false;
        gameEndPanel.gameObject.SetActive(false);
        timeLeft = timeLimit;
        timeLeftText.text = timeLeft.ToString();
        lives = maxLives;
        score = 0;
        scoreText.text = score.ToString();
        correct = 0;
        timeLeftText.color = Color.black;
        UpdateLifeImages();
        GetQuestion();
        gameObject.SetActive(true);
        var animator = GetComponent<Animator>();
        animator.Play("Appear");

        var mainMenuAnimator = mainMenu.GetComponent<Animator>();
        mainMenuAnimator.Play("Disappear");

        if (gameEndPanel.gameObject.activeSelf) {
            var gameEndPanelAnimator = gameEndPanel.GetComponent<Animator>();
            gameEndPanelAnimator.Play("Disappear");
        }
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

        var animator = GetComponent<Animator>();
        animator.Play("Shake");
        lives--;
        ShowTranslation(index);
        buttonImage.color = Color.red;
        UpdateLifeImages();
        if (lives == 0) {
            StartCoroutine(EndGame("Out of Hearts/Hors Des Coeurs!"));
        }
    }

    private void ShowTranslations() {
        wordText.text += " - " + question.answers[question.correct];
        for (var i = 0; i < answers.childCount; i++) {
            ShowTranslation(i);
        }
    }

    private void ShowTranslation(int index) {
        var answerString = question.answers[index];
        var buttonTransform = answers.GetChild(index);
        var text = buttonTransform.GetComponentInChildren<Text>();
        var translation = index == question.correct ? question.question :
            thesaurus.GetRandomTranslationFr(answerString);
        text.text = answerString + " - " + translation;
    }

    private IEnumerator GetQuestionAfterPause() {
        timeLeft += 5;
        yield return new WaitForSeconds(3);
        timeLeftText.text = timeLeft.ToString();
        GetQuestion();
    }

    private void UpdateLifeImages() {
        for (var i = 0; i < maxLives; i++) {
            var image = lifeImages[i];
            image.sprite = i < lives ? lifeImage : missingLifeImage;
        }
    }

    private void CheckTimer() {
        if (gameEnded) {
            return;
        }
        timeLeft--;
        timeLeftText.text = timeLeft.ToString();
        if (timeLeft == 10) {
            timeLeftText.color = new Color(1f, 0.65f, 0f); //orange
        }

        if (timeLeft == 5) {
            timeLeftText.color = Color.red;
        }

        if (timeLeft <= 0) {
            StartCoroutine(EndGame("Time's up/Le temps st écoulé!"));
        }
    }

    private IEnumerator EndGame(string reason) {
        if(gameEnded) {
            yield break;
        }
        gameEnded = true;
        timeLeftText.text = reason;
        score += lives * ScoreManager.pointsPerLife;
        scoreText.text = score.ToString();
        timeLeftText.color = Color.red;
        ShowTranslations();
        yield return new WaitForSeconds(3f);
        gameEndPanel.Show(lives, correct);
    }
}
