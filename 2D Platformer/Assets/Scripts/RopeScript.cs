using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour {


	public Vector2 ropeTarget;

	public float retractSpeed = 2;

	public float distance = 1;

	public float maxDistance = 50;

	public float minDistance = 0.3f;

	public bool pullRopeIn = false;

	public bool hookInterference = false;

	public GameObject ropeNode;

	private List<GameObject> nodes = new List<GameObject> ();

	public LineRenderer line;

	public GameObject hookOrigin;

	private GameObject lastNode;

	private GameObject player;

	private GameObject newNode;

	private Vector3 nodeSpawn;



	void Start(){

		line = this.GetComponent<LineRenderer> ();
		lastNode = this.gameObject;
		nodes.Add (this.gameObject);
		player = GameObject.FindGameObjectWithTag ("Player");

	}

	void Update(){

		nodeSpawn = hookOrigin.transform.rotation * Vector3.forward * .02f;

		if ((Vector2.Distance (this.transform.position, hookOrigin.transform.position) 
			<= maxDistance && !hookInterference) 
			&& !pullRopeIn) {

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
		
		if (Vector2.Distance(lastNode.transform.position, hookOrigin.transform.position) >= distance){
			newNode = (GameObject) Instantiate(ropeNode,
				nodeSpawn,
				Quaternion.identity
			);
			lastNode.GetComponent<ConfigurableJoint> ().connectedBody = newNode.GetComponent<Rigidbody>();
			newNode.GetComponent<ConfigurableJoint> ().connectedBody = hookOrigin.GetComponent<Rigidbody>();
			lastNode = newNode;
		}
	
	}

	void retractRope(){

		if (Vector2.Distance(lastNode.transform.position, hookOrigin.transform.position) <= minDistance) {

		}

	}
}

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