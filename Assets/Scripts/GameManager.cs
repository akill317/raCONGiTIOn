using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public enum GameState {
        menu,
        play,
        clear
    }

    public Mesh[] numberMeshes;

    public GameObject numberPrefab;

    public GameState gameState;

    string numbers = "0123456789";

    float startCountTime;
    public int playTime = 60;
    int originPlayTime;

    public int totalWords = 10;
    int originTotalWords;

    List<GameObject> timeCounter = new List<GameObject>();
    List<GameObject> wordCounter = new List<GameObject>();

    CharacterPainter computer;
    CharacterPainter keyboard;
    CharacterPainter inputField;
    void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start() {
        computer = GameObject.Find("Computer").GetComponent<CharacterPainter>();
        keyboard = GameObject.Find("Keyboard").GetComponent<CharacterPainter>();
        inputField = GameObject.Find("InputField").GetComponent<CharacterPainter>();
        originPlayTime = playTime;
        originTotalWords = totalWords;
        GameOver();
    }

    void Update() {
        if (gameState == GameState.menu) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                GameStart();
            }
        } else if (gameState == GameState.play) {
            if (Time.time - startCountTime >= 1) {
                playTime -= 1;
                startCountTime = Time.time;
                if (playTime >= 0)
                    CountDown(playTime.ToString());
                else
                    GameOver();
            }
        } else if (gameState == GameState.clear) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                GameOver();
            }
        }
    }

    void GameStart() {
        WordGenerator.instance.RemoveAllWords(false);
        startCountTime = Time.time;
        CountDown(playTime.ToString());
        WordGenerator.instance.GenerateWord("", 3.1f, true, true);
        totalWords -= 1;
        WordCountDown(totalWords.ToString());
        gameState = GameState.play;
    }

    public void CountDown(string time) {
        foreach (var chr in timeCounter) {
            Destroy(chr);
        }
        timeCounter.Clear();
        time = time.ToLower();
        float xLeftMost = (1 - time.Length) * 0.6f / 2;
        for (int i = 0; i < time.Length; i++) {
            GameObject chr;
            chr = Instantiate(numberPrefab, new Vector3(xLeftMost + i * 0.6f, 4.2f, 0.6f), Quaternion.Euler(0, 180, 0));
            chr.GetComponent<MeshFilter>().mesh = numberMeshes[numbers.IndexOf(time[i])];
            chr.GetComponent<CharacterPainter>().character = numberMeshes[numbers.IndexOf(time[i])];
            chr.name = time[i].ToString();
            timeCounter.Add(chr);
        }
    }

    public void WordCountDown(string wordNum) {
        foreach (var chr in wordCounter) {
            Destroy(chr);
        }
        wordCounter.Clear();
        wordNum = wordNum.ToLower();
        for (int i = 0; i < wordNum.Length; i++) {
            GameObject chr;
            chr = Instantiate(numberPrefab, new Vector3(-2.7f + i * 0.6f, 4.2f, 0.6f), Quaternion.Euler(0, 180, 0));
            chr.GetComponent<MeshFilter>().mesh = numberMeshes[numbers.IndexOf(wordNum[i])];
            chr.GetComponent<CharacterPainter>().character = numberMeshes[numbers.IndexOf(wordNum[i])];
            chr.name = wordNum[i].ToString();
            wordCounter.Add(chr);
        }
    }

    void RemoveCounter() {
        foreach (var chr in wordCounter) {
            Destroy(chr);
        }
        wordCounter.Clear();
        foreach (var chr in timeCounter) {
            Destroy(chr);
        }
        timeCounter.Clear();
    }

    public void GameClear() {
        gameState = GameState.clear;
        InputFieldController.instance.ClearInputField();
        RemoveCounter();
        DOTween.To(() => computer.shakeAmount, (c) => computer.shakeAmount = c, .1f, 3).SetEase(Ease.InOutElastic);
        DOTween.To(() => keyboard.shakeAmount, (c) => keyboard.shakeAmount = c, 15f, 3).SetEase(Ease.InOutElastic);
        DOTween.To(() => inputField.shakeAmount, (c) => inputField.shakeAmount = c, .1f, 3).SetEase(Ease.InOutElastic);
        WordGenerator.instance.GenerateWord("Clear", 3f, true, false, false);
    }

    public void GameOver() {
        gameState = GameState.menu;
        playTime = originPlayTime;
        totalWords = originTotalWords;
        InputFieldController.instance.ClearInputField();
        RemoveCounter();
        WordGenerator.instance.RemoveAllWords(true);
        WordGenerator.instance.GenerateWord("press", 4.2f, false);
        WordGenerator.instance.GenerateWord("space", 3.2f, false);
        DOTween.To(() => computer.shakeAmount, (c) => computer.shakeAmount = c, 0.001f, 0.5f).SetEase(Ease.InOutElastic);
        DOTween.To(() => keyboard.shakeAmount, (c) => keyboard.shakeAmount = c, 0.001f, 0.5f).SetEase(Ease.InOutElastic);
        DOTween.To(() => inputField.shakeAmount, (c) => inputField.shakeAmount = c, 0.001f, 0.5f).SetEase(Ease.InOutElastic);
    }
}
