using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlatform : MonoBehaviour {
	public GameObject platform;
	private int trig = 0;
	private int midPoint = 60;
	private int trigCap = 120;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {		
		if (trig == trigCap) {
			if (trig < midPoint) {
				transform.position += Vector3.right * 2f;
			} else {
				transform.position += Vector3.left * 2f;
			}
		}
		trig += 1;
		if (trig > trigCap) {
			trig = 0;
		}
	}

	void LateUpdate() {

	}
}
