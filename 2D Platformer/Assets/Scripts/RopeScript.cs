using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour {

	//Speed the hook retracts
	public float retractSpeed = 2;

	//furthest the rope can go
	public float maxDistance = 50;

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
	private Stack<GameObject> nodes = new Stack<GameObject>();

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
		line = this.GetComponent<LineRenderer> ();
		lastNode = this.gameObject;
		nodes.Push (this.gameObject);
		player = GameObject.FindGameObjectWithTag ("Player");
		this.gameObject.GetComponent<Rigidbody> ().useGravity = false;
		hookOrigin = GameObject.FindGameObjectWithTag ("HookOrigin");
		Debug.Log (hookOrigin);
	}

	void Update(){

		nodeSpawn = hookOrigin.transform.rotation * Vector3.forward * .02f;

		if (Vector2.Distance (this.transform.position, hookOrigin.transform.position) 
			<= maxDistance && !pullRopeIn) {

			extendRope ();
		
		} else {
			
			retractRope ();
		
		}	
	}

	void LateUpdate(){

//		for (int x = 0; x < nodes.Count; x++) {
//			nodes3D[x].transform.position = nodes[x].transform.position;
//		}
		
	}
	void extendRope(){
		
		if (Vector2.Distance (lastNode.transform.position, nodeSpawn) >= minDistance) {
			newNode = (GameObject)Instantiate (ropeNode,
				nodeSpawn,
				Quaternion.identity
			);
			lastNode.GetComponent<ConfigurableJoint> ().connectedBody = newNode.GetComponent<Rigidbody> ();
			newNode.GetComponent<ConfigurableJoint> ().connectedBody = hookOrigin.GetComponent<Rigidbody> ();
			lastNode = newNode;
		} else {
			player.GetComponent<HookScript> ().extending = false;
			pullRopeIn = true;
			this.gameObject.GetComponent<Rigidbody> ().useGravity = true;
		}
	
	}

	void retractRope(){
		
		if (Vector2.Distance (lastNode.transform.position, hookOrigin.transform.position) <= minDistance) {
			if (lastNode == this.gameObject) {
				player.GetComponent<HookScript> ().extending = false;
			}

			Destroy (nodes.Pop ());
			lastNode = nodes.Peek ();
			Debug.Log ("Node has been destroyed. Last node is: " + lastNode);
			lastNode.GetComponent<ConfigurableJoint> ().connectedBody = hookOrigin.GetComponent<Rigidbody>();
		}
		lastNode.GetComponent<Rigidbody> ().AddForce (hookOrigin.transform.position
		- lastNode.transform.position * retractSpeed);
	}

	void OnCollisionEnter(Collision col){

		this.gameObject.GetComponent<Rigidbody> ().isKinematic = true;
		
	}
}