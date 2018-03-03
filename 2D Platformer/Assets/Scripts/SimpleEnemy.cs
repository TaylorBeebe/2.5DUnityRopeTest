using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour {
	public GameObject Player;
	int moveSpeed = 3;
	int maxDist = 100;
	int minDist = 5;
	bool canChase = true;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (Player.transform);
		while (Vector3.Distance (transform.position, Player.transform.position) >= minDist) {
			transform.position = Vector3.MoveTowards(this.transform.position, Player.transform.position, moveSpeed * Time.deltaTime);

			if (Vector3.Distance (this.transform.position, Player.transform.position) <= maxDist) {
				break;
			}
		}
		/*
		if (transform.position.x != Player.position.x) {
			canChase == true;
		}
		canChase = false;
		*/
	}

}
