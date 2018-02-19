using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour {
	/*
	public Vector3 destiny;
	public float speed = 1;
	public float distance = 2;
	public GameObject nodePrefab;
	public GameObject player;
	public GameObject lastNode;
	private bool done = false;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		lastNode = transform.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards (transform.position, destiny, speed);

		if ((Vector3)transform.position != destiny) {
			if (Vector3.Distance (player.transform.position, lastNode.transform.position) > distance) {
				CreateNode ();
			}
		} else if (done == false) {
			done = true;
			lastNode.GetComponent<HingeJoint> ().connectedBody = player.GetComponent<Rigidbody> ();
		}
	}
	void CreateNode() {
		Vector3 pos2Create = player.transform.position - lastNode.transform.position;
		pos2Create.Normalize ();
		pos2Create *= distance;
		pos2Create += (Vector3)lastNode.transform.position;

		GameObject go = (GameObject)Instantiate (nodePrefab, pos2Create, Quaternion.identity);

		go.transform.SetParent (transform);

		lastNode.GetComponent<HingeJoint> ().connectedBody = go.GetComponent<Rigidbody> ();
		lastNode = go;
	}
	*/

	public Vector2 ropeTarget;

	public float speed = 2;

	public float distance = 1;

	public GameObject ropeNode2D;

	public GameObject ropeNode3D;

	public List<GameObject> nodes = new List<GameObject>();

	public LineRenderer line;

	public GameObject hookOrigin;

	void Start(){

		line = this.GetComponent<LineRenderer> ();

	}

	void Update(){

		transform.position = Vector2.MoveTowards (transform.position, ropeTarget, speed);

		//if (Vector2.Distance(hookOrigin.transform.position


	
	}
}
