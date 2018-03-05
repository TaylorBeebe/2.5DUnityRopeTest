﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour {

	//Boolean holds if the hook has hit an object or has been interupted
	private bool extending = true;

	//Boolean holds if the hook successfully attached to an object
	private bool hooked = false;

	//Boolean holds if the hook is currently retracting
	private bool retracting = false;

	//Boolean hold if hook is currently retracting due to interference
	private bool interferenceRetracting = false;

	//Boolean holds if hook has reached last node during retraction
	private bool arrivedToLastRetractionNode;

	//Ray used to make sure nothing has come between hook and origin
	private Ray ray;

	//Speed the hook retracts
	public float retractSpeed = 2f;

	//Speed the hook extends
	public float extendSpeed = 2f;

	//Speed hook retracts if interference detected
	public float interferenceRetractSpeed = 5f;

	//furthest the rope can go
	public float maxDistance = 50f;

	//Where hook is heading
	public Vector3 destination;

	//distance constant that sets how far away the last node is before
	//a new node is created
	public float minDistance = 0.3f;

	//renders line between each rope node
	private LineRenderer line;

	//origin of the hook on players arm
	private GameObject hookOrigin;

	//player gameobject
	private GameObject player;

	//List contating all the nodes in the rope
	private Stack<Vector3> nodes = new Stack<Vector3> ();


	void Start(){
		//Gets the line render component
		line = this.GetComponent<LineRenderer> ();

		//Get reference to Player
		player = GameObject.FindGameObjectWithTag ("Player");

		//Get the hook's origin
		hookOrigin = GameObject.FindGameObjectWithTag ("HookOrigin");

		nodes.Push (hookOrigin.transform.position);

		StartCoroutine ("HookMovement");

	}

	void Update(){

		//transform.position = Vector3.MoveTowards (transform.position, destination, extendSpeed * Time.deltaTime);
//		if (!hooked){
//			HookMovement ();
//		}


		if (Vector3.Distance(transform.position, destination) <= 0.2f && !hooked) {

			retracting = true;
			destination = nodes.Peek ();
			StartCoroutine ("HookMovement");

			//Destroy (gameObject);

		}

		//Check if rope was interfered with during deployment, destroy if reached origin 
//		 else if (extending) {
//			if (Physics.Linecast (transform.position, hookOrigin.transform.position, 8)) {
//
//				extending = false;
//				retracting = true;
//				destination = nodes.Peek ();
//
//				this.GetComponent<Collider> ().enabled = false;
//
//				StopCoroutine ("HookMovement");
//				StartCoroutine ("HookMovement");
//			
//			}
//		} else if (hooked) {
//			
//		}


	}
	void LateUpdate(){

//		for (int x = 0; x < nodes.Count; x++) {
//			nodes3D[x].transform.position = nodes[x].transform.position;
//		}
		
	}
	void extendRope(){
		
	
	}

	void retractRope(){

		foreach(Vector3 g in nodes){
			Vector3.MoveTowards (this.transform.position, g, retractSpeed);
		}

	}
	/*
	void OnCollisionEnter(Collision col){
		
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "HookOrigin") {
			Physics.IgnoreCollision (col.collider, this.GetComponent<Collider> ());
		} else {
			Debug.Log ("COLLIDED");
		}

		extending = false;
		hooked = true;
		
	}
	*/
	IEnumerator HookMovement(){
		Debug.Log ("Within Coroutine");
			
		while (true) {
			Debug.Log ((Vector3.Distance (this.transform.position, destination) <= 0.05f));
			Debug.Log (Vector3.Distance (this.transform.position, destination));
			if (Vector3.Distance (this.transform.position, destination) <= 0.05f){
				if (retracting) {
					if (nodes.Count > 1) {
						nodes.Pop ();
						destination = nodes.Peek ();
					} else {
						Destroy (gameObject);
						yield break;
					}
				} else {
					Debug.Log ("REACHED DESTINATION");
					Debug.Log ("Destination: " + destination);
					yield break;
				}
			} else {
				transform.position = Vector3.MoveTowards (transform.position, destination, extendSpeed * Time.deltaTime);
				yield return null;
			}
		}
			
	}
}