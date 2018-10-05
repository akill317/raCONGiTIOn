using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPainter : MonoBehaviour {

    public Mesh character;
    public float shakeAmount;
    public bool drawTrail;
    public bool drawLine;

    List<Vector3> trianglePath = new List<Vector3>();
    List<Vector3> finalPath = new List<Vector3>();

    Vector3[] verticies;
    int[] triangles;
    LineRenderer line;
    Trail trail;

    public bool random = false;
    public Gradient[] possibleColor;
    // Use this for initialization
    void Start() {
        line = GetComponent<LineRenderer>();
        trail = transform.GetComponentInChildren<Trail>();
        if (random && possibleColor.Length > 0) {
            line.colorGradient = possibleColor[Random.Range(0, possibleColor.Length)];
            trail.GetComponent<TrailRenderer>().colorGradient = possibleColor[Random.Range(0, possibleColor.Length)];
        } else if (!random && possibleColor.Length > 0) {
            line.colorGradient = possibleColor[0];
            trail.GetComponent<TrailRenderer>().colorGradient = possibleColor[0];
        }
        GetOriginalTrianglePath();
        trail.originPath = trianglePath.ToArray();
    }

    void Update() {
        if (drawLine) {
            line.enabled = true;
            ShakeLine();
        } else {
            line.enabled = false;
        }

        if (drawTrail) {
            trail.gameObject.SetActive(true);
        } else {
            trail.gameObject.SetActive(false);
        }
    }

    void ShakeLine() {
        if (line.positionCount < trianglePath.Count) {
            line.positionCount = trianglePath.Count;
            line.SetPositions(trianglePath.ToArray());
        }
        for (int i = 0; i < finalPath.Count; i++) {
            finalPath[i] = trianglePath[i] + new Vector3(Mathf.PerlinNoise(Time.time * i * 0.01f, i * 0.01f) * 2 - 1, Mathf.PerlinNoise(i * 0.01f, Time.time * i * 0.01f) * 2 - 1, Mathf.PerlinNoise(Time.time * i * 0.01f, Time.time * i * 0.01f) * 2 - 1) * shakeAmount;
            line.SetPosition(i, finalPath[i]);
        }
    }

    void GetOriginalTrianglePath() {
        verticies = character.vertices;
        triangles = character.triangles;
        foreach (var point in triangles) {
            trianglePath.Add(verticies[point]);
            finalPath.Add(verticies[point]);
        }
    }

    void DrawCharacterInLine() {
        line.positionCount = trianglePath.Count;
        line.SetPositions(trianglePath.ToArray());
    }

}


public static class ShuffleList {
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list) {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}