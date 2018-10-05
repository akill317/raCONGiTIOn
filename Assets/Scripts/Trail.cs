using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Trail : MonoBehaviour {


    public Vector3[] originPath;
    public float speed;
    public float shakeScale;
    public bool allPathOnce;

    bool hasPath;
    int currentPointNum = 0;

    Vector3[] finalPath;
    bool backward = false;
    // Update is called once per frame
    void Update() {
        if (!hasPath && originPath != null) {
            BeginTrail();
        }
        ShakePath();
    }

    void BeginTrail() {
        hasPath = true;
        currentPointNum = 0;
        finalPath = new Vector3[originPath.Length];
        if (allPathOnce)
            transform.DOLocalPath(originPath, speed, PathType.Linear, PathMode.Full3D).SetLoops(-1, LoopType.Yoyo);
        else
            transform.DOLocalMove(originPath[currentPointNum], speed).OnComplete(MoveToNextPoint).SetEase(Ease.Linear);
    }

    void MoveToNextPoint() {
        if (backward) {
            currentPointNum -= 1;
        } else {
            currentPointNum += 1;
        }
        if (currentPointNum >= finalPath.Length && !backward) {
            backward = true;
            currentPointNum = finalPath.Length - 1;
        } else if (currentPointNum <= 0 && backward) {
            backward = false;
            currentPointNum = 0;
        }
        transform.DOLocalMove(finalPath[currentPointNum], speed).OnComplete(MoveToNextPoint).SetEase(Ease.Linear);
    }
    void ShakePath() {
        for (int i = 0; i < finalPath.Length; i++) {
            finalPath[i] = originPath[i] + Random.insideUnitSphere * shakeScale;
        }
    }
}
