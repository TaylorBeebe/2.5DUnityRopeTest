using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {
	public Transform teleporterCounterpart;
	public Transform player;
	double distanceThreshold = 2;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance (transform.position, player.transform.position) <= distanceThreshold) {
			if (Input.GetKeyDown (KeyCode.S)) {
				player.transform.position = teleporterCounterpart.position;
			}
		}
	}
}
