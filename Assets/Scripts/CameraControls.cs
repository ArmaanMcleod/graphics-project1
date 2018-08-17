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

    // Use this for initialization
    void Start() {
        // This locks the cursor to the centre of the screen.
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        var cam = Camera.main.transform;
        // control yaw
        currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
        currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
        // control pitch
        currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
        currentRotation.y = Mathf.Clamp(currentRotation.y, -80f, 80f);
        Camera.main.transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);

    }

    // Accept user input
    private void Move() {
        // Move the camera
        if (Input.GetKey(KeyCode.W)) {

        }

        if (Input.GetKey(KeyCode.S)) {

        }

        if (Input.GetKey(KeyCode.A)) {

        }

        if (Input.GetKey(KeyCode.D)) {

        }
    }
}