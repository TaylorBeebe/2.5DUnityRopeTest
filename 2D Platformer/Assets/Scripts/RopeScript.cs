using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour {

	//player gameobject
	private GameObject player;

	void Start(){
		
		//Get reference to Player
		player = GameObject.FindGameObjectWithTag ("Player");

	}

	void OnCollisionEnter(Collision col){
		Debug.Log ("On collision enter");
		Debug.Log (col.gameObject.tag);
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "HookOrigin") {
			Physics.IgnoreCollision (col.collider, this.GetComponent<Collider> ());
		} else {
			Debug.Log ("COLLIDED");

			player.GetComponent<HookScript> ().hooked = true;
			player.GetComponent<HookScript> ().extending = false;
		}
	}
}