using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour {
	public Rigidbody rigid;
	public SphereCollider col;
	public CharacterJoint ph; 
	public Transform target;
	public float resolution = 0.5F;
	public float ropeDrag = 0.1F;
	public float ropeMass = 0.1F;
	public float ropeColRadius = 0.5F;
	//public float ropeBreakForce = 25.0F;
	private Vector3[] segmentPos;
	private GameObject[] joints;
	private LineRenderer line;
	private int segments = 10;
	private bool rope = false;

	//Joint Settings
	public Vector3 swingAxis = new Vector3(1,0,0);
	public float lowTwistLimit = -100.0F;
	public float highTwistLimit = 100.0F;
	public float swing1Limit = 20.0F;

	void Update() {
		if (Input.GetButtonDown ("Fire1")) {
			BuildRope ();
		} else if (Input.GetButtonDown ("Fire1")) {
			DestroyRope ();
		}
	}

	void FixedUpdate() {
		if (rope) {
			for (int i = 0; i < segments; i++) {
				if (i == 0) {
					line.SetPosition (i, transform.position);
				} else if (i == (segments - 1)) {
					line.SetPosition (i, target.transform.position);
				} else {
					line.SetPosition (i, joints [i].transform.position);
				}
			}
			line.enabled = true;
		} else {
			line.enabled = false;
		}
	}

	void BuildRope() {
		line = gameObject.GetComponent<LineRenderer>();

		segments = (int)(Vector3.Distance (transform.position, target.position) * resolution);
		line.positionCount = segments;
		joints = new GameObject[segments];
		segmentPos[0] = transform.position;
		segmentPos[segments - 1] = target.position;

		// find the distance between each segment
		var segs = segments -1; 
		var seperation = ((target.position - transform.position)/segs);

		for (int s = 1; s < segments; s++) {
			//find each segment's position using the slope from above
			Vector3 vector = (seperation * s) + transform.position;
			segmentPos[s] = vector;

			//Add physics to the segments
			AddJointPhysics(s);
		}

		// Attach the joints to the target object and parent it to this object
		CharacterJoint end = target.gameObject.AddComponent<CharacterJoint>();
		//end.connectedBody = joints[joints.Length-1].transform.rigidbody;
		end.connectedBody = joints[joints.Length-1].transform.GetComponent<Rigidbody>();
		end.swingAxis = swingAxis;
		SoftJointLimit limit_setter = end.lowTwistLimit;
		limit_setter.limit = lowTwistLimit;
		end.lowTwistLimit = limit_setter;
		limit_setter = end.highTwistLimit;
		limit_setter.limit = highTwistLimit;
		end.highTwistLimit = limit_setter;
		limit_setter = end.swing1Limit;
		limit_setter.limit = swing1Limit;
		end.swing1Limit = limit_setter;
		target.parent = transform;

		// rope now exists in scene
		rope = true;
	}

	void AddJointPhysics(int n) {
		joints[n] = new GameObject("Joint_" + n);
		joints[n].transform.parent = transform;
		Rigidbody rigid = joints[n].AddComponent<Rigidbody>();
		SphereCollider col = joints[n].AddComponent<SphereCollider>();
		CharacterJoint ph = joints[n].AddComponent<CharacterJoint>();
		ph.swingAxis = swingAxis;
		SoftJointLimit limit_setter = ph.lowTwistLimit;
		limit_setter.limit = lowTwistLimit;
		ph.lowTwistLimit = limit_setter;
		limit_setter = ph.highTwistLimit;
		limit_setter.limit = highTwistLimit;
		ph.highTwistLimit = limit_setter;
		limit_setter = ph.swing1Limit;
		limit_setter.limit = swing1Limit;
		ph.swing1Limit = limit_setter;
		//

		joints[n].transform.position = segmentPos[n];

		rigid.drag = ropeDrag;
		rigid.mass = ropeMass;
		col.radius = ropeColRadius;

		if(n==1) {
			ph.connectedBody = transform.GetComponent<Rigidbody> ();
			//ph.connectedBody = transform.rigidbody;
		} else {
			ph.connectedBody = joints[n-1].GetComponent<Rigidbody> ();
			//ph.connectedBody = joints[n-1].rigidbody;
		}
	}

		void DestroyRope() {
			//stop rendering rope; then destroy components
			rope = false;
			for(int j = 0; j < joints.Length-1; j++) {
				Destroy(joints[j]);
			}
			segmentPos = new Vector3[0];
			joints = new GameObject[0];
			segments = 0;
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
	void FixedUpdate () {
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

}
*/