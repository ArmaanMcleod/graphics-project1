using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour {

	// Use this for initialization
	private Rigidbody rb;


	void Start () {
		rb = this.gameObject.AddComponent<Rigidbody>();
		rb.useGravity = false;
		rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;


		SphereCollider collider = this.gameObject.AddComponent<SphereCollider>();
		collider.radius = 3; // the larger the radius the less likely it will go through walls at high speed
	}
	
	void FixedUpdate () {
		// Stops the rigid body from rebounding after a collision
		rb.velocity = Vector3.zero;

		
	}
}
