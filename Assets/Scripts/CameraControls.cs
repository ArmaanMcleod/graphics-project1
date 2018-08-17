using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Controls:
 *   w for forward
 *   s for backward
 *   a for left
 *   d for right
 * 
 *   Mouse controls pitch and yaw
 */
public class CameraControls : MonoBehaviour {

    private Vector2 currentRotation;

    public float sensitivity = 1;

    public float moveSpeed = 1;

    // Use this for initialization
    void Start() {
        // This locks the cursor to the centre of the screen.
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        Rotate();
        Move();
    }

    // Accept user rotation input
    private void Rotate() {
        // control yaw
        currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
        currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
        // control pitch
        currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
        currentRotation.y = Mathf.Clamp(currentRotation.y, -80f, 80f);
        transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
    }

    // Accept user movement input
    private void Move() {
        // Move the camera
        if (Input.GetKey(KeyCode.W)) {
            transform.position += relativeForward();
        }

        if (Input.GetKey(KeyCode.S)) {
            transform.position -= relativeForward();
        }

        if (Input.GetKey(KeyCode.A)) {
            transform.position -= relativeRight();
        }

        if (Input.GetKey(KeyCode.D)) {
            transform.position += relativeRight();
        }

    }

    // Returns the relative forward vector scaled by the movement speed and time delta
    private Vector3 relativeForward() {
        return transform.forward * moveSpeed * Time.deltaTime;
    }

    // Returns the relative right vector scaled by the movement speed and time delta
    private Vector3 relativeRight() {
        return transform.right * moveSpeed * Time.deltaTime;
    }
}