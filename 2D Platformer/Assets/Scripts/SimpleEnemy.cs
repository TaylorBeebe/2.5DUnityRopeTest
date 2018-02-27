using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour {
	public Transform Player;
	int moveSpeed = 3;
	int maxDist = 100;
	int minDist = 5;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (Player);
		if (Vector3.Distance (transform.position, Player.position) >= minDist) {
			transform.position = Vector3.MoveTowards(this.transform.position, Player, moveSpeed * Time.deltaTime);

			if (Vector3.Distance (transform.position, Player.position) <= maxDist) {
				
			}
		}
	}
}
