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

	//Detects if hook has collided
	void OnCollisionEnter(Collision col){


//		Debug.Log ("On collision enter");
//		Debug.Log (col.gameObject.tag);

		//If hook has collided with the player or the origin cube, ignore the collision
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "HookOrigin") {
			Physics.IgnoreCollision (col.collider, this.GetComponent<Collider> ());
		} else {
			
//			Debug.Log ("COLLIDED");

			this.gameObject.GetComponent<Rigidbody> ().isKinematic = true;

			//Tell the hookscript that the hook has collided
			player.GetComponent<HookScript> ().hooked = true;
			player.GetComponent<HookScript> ().extending = false;
		}
	}
}