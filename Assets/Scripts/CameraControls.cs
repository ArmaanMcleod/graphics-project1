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

    public int reboundDistance;

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
        CheckBounds();
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
            transform.position += RelativeForward();
        }

        if (Input.GetKey(KeyCode.S)) {
            transform.position -= RelativeForward();
        }

        if (Input.GetKey(KeyCode.A)) {
            transform.position -= RelativeRight();
        }

        if (Input.GetKey(KeyCode.D)) {
            transform.position += RelativeRight();
        }

    }

    // Returns the relative forward vector scaled by the movement speed and time delta
    private Vector3 RelativeForward() {
        return transform.forward * moveSpeed * Time.deltaTime;
    }

    // Returns the relative right vector scaled by the movement speed and time delta
    private Vector3 RelativeRight() {
        return transform.right * moveSpeed * Time.deltaTime;
    }

    private void CheckBounds() {
        // Get terrain object
        GameObject terrainObject = GameObject.Find("Terrain");
        DiamondSquareTerrain terrain = terrainObject.GetComponent<DiamondSquareTerrain>();

        // Copy current position
        Vector3 currentPostion = transform.localPosition;

        // Sides closest to origin
        if (currentPostion.x < reboundDistance) {
            currentPostion.x = reboundDistance;
        }

        if (currentPostion.z < reboundDistance) {
            currentPostion.z = reboundDistance;
        }

        // Sides furthest from origin
        if (currentPostion.x > terrain.getSize() - reboundDistance) {
            currentPostion.x = terrain.getSize() - reboundDistance;
        }

        if (currentPostion.z > terrain.getSize() - reboundDistance) {
            currentPostion.z = terrain.getSize() - reboundDistance;
        }

        // Update position with new position
        transform.localPosition = currentPostion;
    }

}