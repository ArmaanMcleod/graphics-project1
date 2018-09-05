using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to detect collison detection of the camera.
/// </summary>
[RequireComponent (typeof (SphereCollider))]
[RequireComponent (typeof (Rigidbody))]
public class CameraCollision : MonoBehaviour {

    // Rigid body
    private Rigidbody rb;

    // radius of sphere
    public int radius = 3;

    /// <summary>
    /// Used for initialisation.
    /// </summary>
    void Start () {
        // Create rigid body
        rb = this.gameObject.AddComponent<Rigidbody> ();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        // Create sphere collider for collision detection
        SphereCollider collider = this.gameObject.AddComponent<SphereCollider> ();

        // The larger the radius the less likely it will go through walls at high speed
        collider.radius = radius;
    }

    /// <summary>
    /// Called every fixed framerate frame. Adds reaction when collision occurs
    /// between the camera and terrain. 
    /// </summary>
    void FixedUpdate () {

        // Stops the rigid body from rebounding after a collision
        rb.velocity = Vector3.zero;

        // Stops the rigid body from rotating after a collision
        rb.angularVelocity = Vector3.zero;
    }
}