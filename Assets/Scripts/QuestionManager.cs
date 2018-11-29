using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class QuestionManager : MonoBehaviour {
    public Thesaurus thesaurus;
    public int numAnswers = 5;

    public Question GetQuestion() {
        string word, correctWord;
        do {
            word = thesaurus.GetRandomWordEn();
            correctWord = thesaurus.GetRandomTranslationEn(word);
        } while (word == correctWord);//skip over the very easy ones.

        var answers = new List<string>();
        for (var i = 0; i < numAnswers - 1; i++) {
            var answer = thesaurus.GetRandomWordFr();
            if (answer == correctWord) {
                i--;
                continue;
            }
            answers.Add(answer);
        }
        var correct = UnityEngine.Random.Range(0, numAnswers);
        answers.Insert(correct, correctWord);

        return new Question {
            question = word,
            answers = answers,
            correct = correct
        };
    }
}
