using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;

public class Thesaurus : MonoBehaviour {
    public TextAsset thesaurusFile;

    public readonly Dictionary<string, HashSet<string>> en2fr =
        new Dictionary<string, HashSet<string>>();
    public readonly Dictionary<string, HashSet<string>> fr2en =
        new Dictionary<string, HashSet<string>>();

    private void Awake() {
        LoadFile();
    }

    private void LoadFile() {
        var lines = thesaurusFile.text.Split('\n');
        foreach (var line in lines) {
            ProcessLine(line);
        }
    }

    private Regex mfMarker = new Regex(@" \{[mf](.pl)?\}");
    private Regex angleMarker = new Regex(@" \<[^>]+\>");
    private Regex squareBrackets = new Regex(@" \[[^\]]+\]");

    private void ProcessLine(string line) {
        if (line.StartsWith("#")) {
            return;
        }
        if (line == "") {
            return;
        }
        var cols = line.Split('\t');
        var english = cols[0];
        var french = cols[1];
        //var partOfSpeech = cols[2];
        var tags = cols[3];
        if (tags.Contains("[zool.]") || tags.Contains("[orn.]")) {
            english = squareBrackets.Replace(english, "");
            french = squareBrackets.Replace(french, "");
        }

        english = angleMarker.Replace(english, "");
        english = english.Replace(" {pl}", "");
        english = english.Replace(" [Am.]", "");
        english = english.Replace(" [Br.]", "");
        english = english.Replace(" [coll.]", "");
        english = english.Replace(" [no pl]", "");
        english = english.Replace(" [also fig.]", "");
        english = english.Replace(" [fig.]", "");
        english = english.Replace(" [pej.]", "");
        english = english.Replace(" {sg}", "");
        english = english.Replace("[", "<i>").Replace("]", "</i>");
        english = english.Replace("sb.", "<i>someone</i>");
        english = english.Replace("sth.", "<i>something</i>");
        french = mfMarker.Replace(french, "");
        french = angleMarker.Replace(french, "");
        french = french.Replace(" [fam.]", "");
        french = french.Replace(" [can.]", "");
        french = french.Replace(" [pej.]", "");
        french = french.Replace("[", "<i>").Replace("]", "</i>");
        french = french.Replace("qn.", "<i>quelqu'un</i>");
        french = french.Replace("qc.", "<i>quelque chose</i>");
        HashSet<string> set;
        if (en2fr.TryGetValue(english, out set)) {
            set.Add(french);
        }
        else {
            en2fr.Add(english, new HashSet<string>{
                french
            });
        }


        if (fr2en.TryGetValue(french, out set)) {
            set.Add(english);
        }
        else {
            fr2en.Add(french, new HashSet<string>{
                english
            });
        }
    }

    public string GetRandomWordEn() {
        var index = UnityEngine.Random.Range(0, en2fr.Count);
        return en2fr.Keys.ElementAt(index);
    }


    public string GetRandomWordFr() {
        var index = UnityEngine.Random.Range(0, fr2en.Count);
        return fr2en.Keys.ElementAt(index);
    }

    public string GetRandomTranslationEn(string word) {
        var translations = en2fr[word];
        if (translations.Count == 1) {
            return translations.First();
        }
        var index = UnityEngine.Random.Range(0, translations.Count);
        return translations.ElementAt(index);
    }

    public string GetRandomTranslationFr(string word) {
        var translations = fr2en[word];
        if (translations.Count == 1) {
            return translations.First();
        }
        var index = UnityEngine.Random.Range(0, translations.Count);
        return translations.ElementAt(index);
    }
}
