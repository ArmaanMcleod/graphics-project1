using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour {

	// Use this for initialization
	void Update () {
		// Stops the rigid body from rebounding during a collision
		this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

		
	}

	void OnCollisionEnter(Collision c){
		Debug.Log(this.gameObject.GetComponent<Rigidbody>().velocity);
		this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
	
	}
}
