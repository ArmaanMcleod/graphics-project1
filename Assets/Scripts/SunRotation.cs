using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When attached to a Sun sphere, this rotates the object around the center of a map.
/// </summary>
[RequireComponent (typeof (Renderer))]
[RequireComponent (typeof (DiamondSquareTerrain))]
public class SunRotation : MonoBehaviour {

    // Speed of sun
    public float speed = 50.0f;

    // Scale size for sun
    public float scale = 20.0f;

    // Color of object
    private Color color;

    /// <summary>
    /// Use this for initialization
    /// </summary>
    private void Start () {

        // Multiple x, y, z scales ot make object bigger
        float xScale = this.transform.localScale.x * scale;
        float yScale = this.transform.localScale.y * scale;
        float zScale = this.transform.localScale.z * scale;

        this.transform.localScale = new Vector3 (xScale, yScale, zScale);

        // Extract colour component
        color = GetComponent<Renderer> ().material.color;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update () {

        // Get size of terrain
        GameObject terrainObject = GameObject.Find ("Terrain");
        DiamondSquareTerrain terrain = terrainObject.GetComponent<DiamondSquareTerrain> ();
        float terrainSize = terrain.GetSize () * 2;

        // Centre position of map
        Vector3 position = new Vector3 (terrainSize / 2, 0, terrainSize / 2);

        // Rotation around centre
        transform.RotateAround (position, Vector3.forward, speed * Time.deltaTime);

    }

    /// <summary>
    /// Gets the color of the sun.
    /// </summary>
    /// <returns>The color of the sun</returns>
    public Color GetColor () {
        return color;
    }

    /// <summary>
    /// Gets the current position of the sun in World space.
    /// </summary>
    /// <returns>A vector position</returns>
    public Vector3 GetWorldPosition () {
        return transform.position;
    }
}