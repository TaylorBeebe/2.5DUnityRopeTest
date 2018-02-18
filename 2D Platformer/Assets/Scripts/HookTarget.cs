using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookTarget : MonoBehaviour {
	public float step = 1000;
	// Use this for initialization
	void Start () {
		PositionUpdate ();
	}
	
	// Update is called once per frame
	void Update () {
		PositionUpdate ();		
	}

	void PositionUpdate() {
		var v3 = Input.mousePosition;
		v3.z = 0;
		v3 = Camera.main.ScreenToWorldPoint (v3);
		v3.z = 0;
		this.transform.position = Vector3.MoveTowards (this.transform.position, v3, step);			
	}
}
