using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainDropSpawner : MonoBehaviour {

    public GameObject rainDrop;

    Collider col;

    // Use this for initialization
    void Start() {
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update() {
        if (Time.frameCount % 1 == 0) {
            SpawnRainDrop();
        }
    }

    void SpawnRainDrop() {
        Vector3 randomPoint = col.bounds.center +
            new Vector3(Random.Range(-col.bounds.extents.x, col.bounds.extents.x), Random.Range(-col.bounds.extents.y, col.bounds.extents.y), Random.Range(-col.bounds.extents.z, col.bounds.extents.z));
        Instantiate(rainDrop, randomPoint, Quaternion.identity, transform);
    }
}
