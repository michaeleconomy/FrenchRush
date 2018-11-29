using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class QuestionManager : MonoBehaviour {
    public Thesaurus thesaurus;
    public int numAnswers = 5;

    public Question GetQuestion() {
        var word = thesaurus.GetRandomWordEn();
        var answers = new List<string>();
        for (var i = 0; i < numAnswers - 1; i++) {
            answers.Add(thesaurus.GetRandomWordFr());
        }
        var correctWord = thesaurus.GetRandomTranslationEn(word);
        var correct = UnityEngine.Random.Range(0, numAnswers);
        answers.Insert(correct, correctWord);

        return new Question {
            question = word,
            answers = answers,
            correct = correct
        };
    }
}
