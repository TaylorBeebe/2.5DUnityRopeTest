using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour {

	//Boolean checks if the hook has hit an object or has been interupted
	private bool extending = true;

	private bool hooked = false;

	//Boolean checks if hook is currently retracting due to interference
	private bool interferenceRetracting = false;

	private bool hookLatched = false;

	//Ray used to make sure nothing has come between hook and origin
	private Ray ray;

	//Speed the hook retracts
	public float retractSpeed = 2f;

	//Speed hook retracts if interference detected
	public float interferenceRetractSpeed = 5f;

	//furthest the rope can go
	public float maxDistance = 50f;

	//distance constant that sets how far away the last node is before
	//a new node is created
	public float minDistance = 0.3f;

	//set to true when the player has set the rope to retract or
	//the rope has hit interference
	public bool pullRopeIn = false;

	//prefab to instantiate new nodes in the rope
	public GameObject ropeNode;

	//stores a list of all nodes currently in the rope
	//private List<GameObject> nodes = new List<GameObject> ();
	private List<GameObject> nodes = new List<GameObject>();

	//renders line between each rope node
	private LineRenderer line;

	//origin of the hook on players arm
	private GameObject hookOrigin;

	//last node created. initially set to be the hook
	private GameObject lastNode;

	//player gameobject
	private GameObject player;

	//used when instantiating a new node
	private GameObject newNode;

	//spawn location of new nodes. set to be slightly in front of hookOrigin
	private Vector3 nodeSpawn;



	void Start(){
		//Gets the line render component
		line = this.GetComponent<LineRenderer> ();

		//Get reference to Player
		player = GameObject.FindGameObjectWithTag ("Player");

		//Get the hook's origin
		hookOrigin = GameObject.FindGameObjectWithTag ("HookOrigin");
	}

	void Update(){
		
		//Check if rope was interfered with during deployment, destroy if reached origin 
		if (interferenceRetracting) {
			if (Vector3.Distance (this.transform.position, hookOrigin.transform.position) <= 0.2f) {
				Destroy (this);
			}
		} 

		else if (extending) {
			if (Physics.Linecast (transform.position, hookOrigin.transform.position, 8) ||
				Vector3.Distance(this.transform.position, hookOrigin.transform.position) >= maxDistance) {
				extending = false;
				interferenceRetracting = true;
				this.GetComponent<Collider> ().enabled = false;
				Vector3.MoveTowards (transform.position, hookOrigin.transform.position, interferenceRetractSpeed);
			}
		}


	}
	void LateUpdate(){

//		for (int x = 0; x < nodes.Count; x++) {
//			nodes3D[x].transform.position = nodes[x].transform.position;
//		}
		
	}
	void extendRope(){
		
	
	}

	void retractRope(){


	}

	void OnCollisionEnter(Collision col){

		this.gameObject.GetComponent<Rigidbody> ().isKinematic = true;

		hooked = true;
		
	}

}