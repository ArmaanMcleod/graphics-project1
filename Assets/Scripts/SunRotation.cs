using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When attached to a Sun sphere, this rotates the object around the center of a map.
/// </summary>
public class SunRotation : MonoBehaviour {

    // Speed of sun
    public float speed;

    // Scale size for sun
    public float scale;

    // Color of object
    private Color color;

    // Centre position in map
    private Vector3 centerPosition;

    // Radius of rotation
    private float radiusRotation;

    // Speed of rotation radius
    public float radiusSpeed;

    /// <summary>
    /// Use this for initialization
    /// </summary>
    private void Start () {
        // Obtain terrain obejct
        GameObject terrainObject = GameObject.Find ("Terrain");
        Terrain terrain = terrainObject.GetComponent<Terrain> ();

        // Get terrain center point
        float center = terrain.terrainData.heightmapWidth / 2;
        centerPosition = new Vector3 (center, 0.0f, center);

        // Set radius as maximum height
        radiusRotation = terrain.terrainData.heightmapHeight;

        // Update position 
        this.transform.position = GetNewPosition ();

        // Multiple x, y, z scales ot make object bigger
        float xScale = this.transform.localScale.x * scale;
        float yScale = this.transform.localScale.y * scale;
        float zScale = this.transform.localScale.z * scale;
        this.transform.localScale = new Vector3 (xScale, yScale, zScale);

        // Extract colour component
        color = this.gameObject.GetComponent<Renderer> ().material.color;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update () {

        // Rotation around center
        transform.RotateAround (centerPosition, Vector3.forward, speed * Time.deltaTime);

        // Move towards desired position
        transform.position = Vector3.MoveTowards (transform.position, GetNewPosition (), Time.deltaTime * radiusSpeed);
    }

    /// <summary>
    /// Gets the color of the sun.
    /// </summary>
    /// <returns>The color of the sun</returns>
    public Color GetColor () {
        return color;
    }

    /// <summary>
    /// Gets new position with respect to radius
    /// </summary>
    /// <returns>New vector position in world</returns>
    private Vector3 GetNewPosition () {
        return (transform.position - centerPosition).normalized * radiusRotation + centerPosition;
    }

    /// <summary>
    /// Gets the current position of the sun in World space.
    /// </summary>
    /// <returns>A vector position</returns>
    public Vector3 GetWorldPosition () {
        return transform.position;
    }
}