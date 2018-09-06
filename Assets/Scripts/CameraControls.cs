using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls the main camera within the terrain. It enables basic
/// movement with w,a,s,d and the mouse rotation.
/// 
/// Controls:
/// * W for forward
/// * S for backward
/// * A for left
/// * D for right
/// 
/// * Mouse controls pitch and yaw.
/// </summary>
public class CameraControls : MonoBehaviour {

    // Current rotation of camera
    private Vector2 currentRotation;

    // Minimum distance allowed from edge of terrain
    public float reboundDistance;

    // Sensitivity of camera
    public float sensitivity;

    // Speed of camera
    public float moveSpeed;

    /// <summary>
    /// Used for initialisation of camera.
    /// </summary>
    private void Start () {
        // This locks the cursor to the centre of the screen
        Cursor.lockState = CursorLockMode.Locked;

        // Start up location 
        this.transform.position = new Vector3 (0.0f, 100.0f, 0.0f);

    }

    /// <summary>
    /// This is called once per frame to update camera movements.
    /// </summary>
    private void Update () {

        // Rotation
        Rotate ();

        // Movement
        Move ();

        // Bounds checking
        CheckBounds ();
    }

    /// <summary>
    /// Handles user rotation input.
    /// </summary>
    private void Rotate () {
        // Control yaw
        currentRotation.x += Input.GetAxis ("Mouse X") * sensitivity;
        currentRotation.x = Mathf.Repeat (currentRotation.x, 360);

        // Control pitch
        currentRotation.y -= Input.GetAxis ("Mouse Y") * sensitivity;
        currentRotation.y = Mathf.Clamp (currentRotation.y, -80f, 80f);
        transform.rotation = Quaternion.Euler (currentRotation.y, currentRotation.x, 0);
    }

    /// <summary>
    /// Handles user movement input.
    /// </summary>
    private void Move () {

        // Up
        if (Input.GetKey (KeyCode.W)) {
            transform.position += RelativeForward ();
        }

        // Down
        if (Input.GetKey (KeyCode.S)) {
            transform.position -= RelativeForward ();
        }

        // Left
        if (Input.GetKey (KeyCode.A)) {
            transform.position -= RelativeRight ();
        }

        // Right
        if (Input.GetKey (KeyCode.D)) {
            transform.position += RelativeRight ();
        }

    }

    /// <summary>
    /// Returns the relative forward vector scaled by the movement speed
    /// and time delta.
    /// </summary>
    /// <returns>Relative forward vector</returns>
    private Vector3 RelativeForward () {
        return transform.forward * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Returns the relative right vector scaled by the movement speed 
    /// and time delta.
    /// </summary>
    /// <returns>Relative right vector</returns>
    private Vector3 RelativeRight () {
        return transform.right * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Ensures camera stays within bounds of terrain.
    /// </summary>
    private void CheckBounds () {
        // Get terrain object
        GameObject terrainObject = GameObject.Find ("Terrain");
        Terrain terrain = terrainObject.GetComponent<Terrain> ();

        // Get terrain size
        float terrainSize = terrain.terrainData.heightmapWidth;

        // Copy current position
        Vector3 currentPostion = transform.position;

        // Sides closest to origin
        if (currentPostion.x < reboundDistance) {
            currentPostion.x = reboundDistance;
        }

        if (currentPostion.z < reboundDistance) {
            currentPostion.z = reboundDistance;
        }

        // Sides furthest from origin
        if (currentPostion.x > terrainSize - reboundDistance) {
            currentPostion.x = terrainSize - reboundDistance;
        }

        if (currentPostion.z > terrainSize - reboundDistance) {
            currentPostion.z = terrainSize - reboundDistance;
        }

        // Update position with new position
        transform.position = currentPostion;
    }

}