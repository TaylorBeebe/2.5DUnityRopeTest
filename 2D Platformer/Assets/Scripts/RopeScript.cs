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

	public GameObject hookNode3D;

	private List<GameObject> nodes = new List<GameObject>();

	private List<GameObject> nodes3D = new List<GameObject> ();

	private Vector3[] linePoints = new Vector3[100];

	public LineRenderer line;

	public Rigidbody2D hookOrigin;

	private GameObject lastNode;

	private GameObject player;

	private GameObject newNode;

	void Start(){

		line = this.GetComponent<LineRenderer> ();
		lastNode = this.gameObject;
		Instantiate (hookNode3D, this.transform);
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void Update(){

		transform.position = Vector2.MoveTowards (transform.position, ropeTarget, speed);

		if (Vector2.Distance(lastNode.transform.position, hookOrigin.transform.position) >= distance){
			newNode = (GameObject) Instantiate(ropeNode2D, 
				new Vector3(hookOrigin.transform.position.x, hookOrigin.transform.position.y, 0f),
				Quaternion.identity
			);
			lastNode.GetComponent<HingeJoint2D> ().connectedBody = newNode.GetComponent<Rigidbody2D>();
			newNode.GetComponent<HingeJoint2D> ().connectedBody = hookOrigin.GetComponent<Rigidbody2D> ();
		}


	
	}

	void LateUpdate(){

		hookNode3D.transform.position = this.gameObject.transform.position;
		
		for (int x = 0; x < nodes.Count; x++) {
			nodes3D[x].transform.position = nodes[x].transform.position;
		}
		
	}
}
