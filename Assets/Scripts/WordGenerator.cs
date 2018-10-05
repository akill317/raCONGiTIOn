using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WordGenerator : MonoBehaviour {

    public static WordGenerator instance;

    public Mesh[] upperCaseWordMesh;
    public Mesh[] lowerCaseWordMesh;

    public GameObject upperCaseLetterPrefab;
    public GameObject lowerCaseLetterPrefab;

    public TextAsset wordsFile;

    string alphabet = "abcdefghijklmnopqrstuvwxyz";
    string oneLongString;
    string[] thousandWords;

    List<string> acceptableWords = new List<string>();
    List<GameObject> instantiatedChr = new List<GameObject>();
    public string instantiatedWord;
    void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start() {
        GetAcceptableWords();
    }

    void GetAcceptableWords() {
        oneLongString = wordsFile.text;
        thousandWords = oneLongString.Split('\n');
        foreach (var word in thousandWords) {
            if (word.Length >= 3 && word.Length <= 9) {
                acceptableWords.Add(word);
            }
        }
    }

    //3 for instantiated word
    public void GenerateWord(string word, float height, bool scaleIn, bool random = false, bool trail = true) {
        if (random)
            word = acceptableWords[Random.Range(0, acceptableWords.Count)];
        word = word.ToLower();
        float xLeftMost = (1 - word.Length) * 0.7f / 2;
        for (int i = 0; i < word.Length; i++) {
            GameObject chr;
            if (Random.value > 0.5) {
                chr = Instantiate(upperCaseLetterPrefab, new Vector3(xLeftMost + i * 0.7f, height, 0.6f), Quaternion.Euler(0, 180, 0));
                chr.GetComponent<MeshFilter>().mesh = upperCaseWordMesh[alphabet.IndexOf(word[i])];
                chr.GetComponent<CharacterPainter>().character = upperCaseWordMesh[alphabet.IndexOf(word[i])];
            } else {
                chr = Instantiate(lowerCaseLetterPrefab, new Vector3(xLeftMost + i * 0.7f, height, 0.6f), Quaternion.Euler(90, 180, 0));
                chr.name = word[i].ToString();
                chr.GetComponent<MeshFilter>().mesh = lowerCaseWordMesh[alphabet.IndexOf(word[i])];
                chr.GetComponent<CharacterPainter>().character = lowerCaseWordMesh[alphabet.IndexOf(word[i])];
            }
            chr.name = word[i].ToString();
            chr.GetComponent<CharacterPainter>().drawTrail = trail;
            chr.GetComponent<CharacterPainter>().drawLine = !trail;
            if (scaleIn) {
                Vector3 originScale = chr.transform.localScale;
                chr.transform.localScale = Vector3.zero;
                chr.transform.DOScale(originScale.x, 1f).SetEase(Ease.OutQuad);
            }
            instantiatedChr.Add(chr);
        }
        instantiatedWord = word;
    }

    public void RemoveAllWords(bool fade) {
        foreach (var chr in instantiatedChr) {
            if (fade)
                chr.transform.DOScale(0, 1f).SetEase(Ease.OutCubic).OnComplete(() => { Destroy(chr); });
            else
                Destroy(chr);
        }
        instantiatedChr.Clear();
        instantiatedWord = "";
    }
}
