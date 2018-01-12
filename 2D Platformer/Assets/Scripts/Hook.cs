using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 
public class Hook : MonoBehaviour {
	public Transform parentTransform;
	private RaycastHit hit;
	public Rigidbody rb;
	public bool hooked = false;
	private float momentum;
	public float speed;
	private float step;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var v3 = Input.mousePosition;
		v3.z = 0;
		v3 = Camera.main.ScreenToWorldPoint (v3);
		v3.z = 0;
		this.transform.LookAt (v3);
		if (Input.GetButtonDown ("Fire1")) {
			if(Physics.Raycast(cubeTransformPosition(), this.transform.rotation * Vector3.forward, out hit)) {
				hooked = true;
				rb.isKinematic = true;
			}				
		}
		if (Input.GetButtonUp ("Fire1")) {
			if (hooked) {
				hooked = false;
				rb.isKinematic = false;
				rb.velocity = this.transform.rotation * Vector3.forward * momentum;
			}
		}
		if (hooked) {
			momentum += Time.deltaTime * speed;
			step = momentum * Time.deltaTime;
			parentTransform.position = Vector3.MoveTowards (parentTransform.position, hit.point, step);
		} else if (momentum >= 0) {
			momentum -= Time.deltaTime * 5;
			step = 0;
		}
	}

	Vector3 cubeTransformPosition(){
		return new Vector3 (this.transform.position.x, this.transform.position.y, 0);
	}

			
	void FixedUpdate() {
		//if (!hooked) {
		//}
	}
}
