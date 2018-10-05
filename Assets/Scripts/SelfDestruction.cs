using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruction : MonoBehaviour {
    public float deadTime = 1;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, deadTime);
	}
	
}
