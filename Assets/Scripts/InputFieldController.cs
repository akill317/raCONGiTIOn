using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFieldController : MonoBehaviour {

    public static InputFieldController instance;
    public Mesh[] alphabetLowerCase;
    public GameObject alphabetPrefab;

    public List<GameObject> inputChar = new List<GameObject>();
    public string inputString;

    string alphabet;

    void Awake() {
        instance = this;
    }
    // Use this for initialization
    void Start() {
        alphabet = "abcdefghijklmnopqrstuvwxyz";
    }

    // Update is called once per frame
    void Update() {
        if (GameManager.instance.gameState != GameManager.GameState.play) return;

        for (int i = 0; i < 26; i++) {
            if (Input.GetKeyDown((KeyCode)(i + 97))) {
                if (inputChar.Count < 9) {
                    AddChar(i);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            if (inputChar.Count > 0)
                RemoveChar();
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            SubmitAnswer();
        }
    }

    void AddChar(int character) {
        GameObject c = Instantiate(alphabetPrefab, new Vector3(-1f + inputChar.Count * 0.25f, transform.position.y, 0.6f), Quaternion.Euler(0, 180, 0));
        c.GetComponent<MeshFilter>().mesh = alphabetLowerCase[character];
        c.GetComponent<CharacterPainter>().character = alphabetLowerCase[character];
        inputString = inputString.Insert(inputString.Length, alphabet[character].ToString());
        inputChar.Add(c);
    }

    void RemoveChar() {
        Destroy(inputChar[inputChar.Count - 1]);
        inputChar.RemoveAt(inputChar.Count - 1);
        inputString = inputString.Remove(inputString.Length - 1);
    }

    void SubmitAnswer() {
        if (inputString == WordGenerator.instance.instantiatedWord.ToLower()) {
            Correct();
        } else {
            Wrong();
            return;
        }
        ClearInputField();
    }

    public void ClearInputField() {
        foreach (var chr in inputChar) {
            Destroy(chr);
        }
        inputChar.Clear();
        inputString = "";
    }

    void Correct() {
        WordGenerator.instance.RemoveAllWords(false);
        if (GameManager.instance.totalWords > 0) {
            GameManager.instance.totalWords -= 1;
            GameManager.instance.WordCountDown(GameManager.instance.totalWords.ToString());
            WordGenerator.instance.GenerateWord("", 3.1f, true, true);
        } else {
            GameManager.instance.GameClear();
        }
    }

    void Wrong() {

    }
}
