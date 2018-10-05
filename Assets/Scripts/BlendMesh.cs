using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BlendMesh : MonoBehaviour {

    [Range(0, 1)]
    public float weight;
    public Mesh mesh1;
    public Mesh mesh2;

    TrailRenderer trail;
    float lastWeight;
    List<Vector3> newVertexList = new List<Vector3>();
    // Use this for initialization
    void Start() {
        trail = GetComponentInChildren<TrailRenderer>();
        SetupLine();
    }

    // Update is called once per frame
    void Update() {
        if (weight != lastWeight) {
            lastWeight = weight;
            UpdateLine();
        }
    }

    void SetupLine() {
        var vtx1 = mesh1.vertices;
        foreach (var vertex in vtx1) {
            newVertexList.Add(transform.TransformPoint(vertex));
        }
        var path = new List<Vector3>();
        foreach (var tri in mesh1.triangles) {
            path.Add(newVertexList[tri]);
        }
        var curve = path.ToArray();
        trail.transform.DOLocalPath(curve, 5).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    void UpdateLine() {
        var vtx1 = mesh1.vertices;
        var vtx2 = mesh2.vertices;
        foreach (var vertex in mesh1.triangles) {
            Debug.Log(vertex);
        }
        newVertexList.Clear();
        int vertexNum = Mathf.FloorToInt(vtx1.Length * (1 - weight) + vtx2.Length * weight);
        for (int i = 0; i < vertexNum; i++) {
            Vector3 newVertex = Vector3.zero;
            if (vtx1.Length > i && vtx2.Length > i) {
                newVertex = vtx1[i] * (1 - weight) + vtx2[i] * weight;
            } else if (vtx1.Length > i && vtx2.Length <= i) {
                newVertex = vtx1[i];
            } else if (vtx2.Length > i && vtx1.Length <= i) {
                newVertex = vtx2[i];
            }
            newVertexList.Add(transform.TransformPoint(newVertex));
        }
        trail.DOKill();
        var curve = newVertexList.ToArray();
        //OnComplete(() => { trail.transform.DOLocalMove(curve[0], 0.1f); })
        trail.transform.DOLocalPath(curve, 10).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    Vector3[] Chaikin(Vector3[] pts) {
        Vector3[] newPts = new Vector3[(pts.Length - 2) * 2 + 2];
        newPts[0] = pts[0];
        newPts[newPts.Length - 1] = pts[pts.Length - 1];

        int j = 1;
        for (int i = 0; i < pts.Length - 2; i++) {
            newPts[j] = pts[i] + (pts[i + 1] - pts[i]) * 0.75f;
            newPts[j + 1] = pts[i + 1] + (pts[i + 2] - pts[i + 1]) * 0.25f;
            j += 2;
        }
        return newPts;
    }
}
