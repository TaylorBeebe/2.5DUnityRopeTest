using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRotate : MonoBehaviour {


	
	// Update is called once per frame
	void LateUpdate () {
		if (Input.GetKeyDown("b")) {
			transform.Rotate (Vector3.up, 100);
		}
	}
}
