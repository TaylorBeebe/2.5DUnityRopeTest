using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {
	public Transform teleporterCounterpart;
	public Transform player;
	int distanceThreshold = 5;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance (transform.position, player.position) >= distanceThreshold) {
			if (Input.GetButtonDown (KeyCode.W)) {
				player.position = teleporter1.position;
			}
		}
	}
}
