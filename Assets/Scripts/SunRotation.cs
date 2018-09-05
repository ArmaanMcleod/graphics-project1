using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotation : MonoBehaviour {

    // Speed of sun
    public float speed = 50.0f;

    // Scale size for sun
    public float scale = 20.0f;

    private Color color;

    // Use this for initialization
    void Start () {

        // Multiple x, y, z scales ot make object bigger
        float xScale = this.transform.localScale.x * scale;
        float yScale = this.transform.localScale.y * scale;
        float zScale = this.transform.localScale.z * scale;

        this.transform.localScale = new Vector3 (xScale, yScale, zScale);

        color = GetComponent<Renderer> ().material.color;

    }

    // Update is called once per frame
    void Update () {

        // Get size of terrain
        GameObject terrainObject = GameObject.Find ("Terrain");
        DiamondSquareTerrain terrain = terrainObject.GetComponent<DiamondSquareTerrain> ();
        float terrainSize = terrain.GetSize ();

        // Centre position of map
        Vector3 position = new Vector3 (terrainSize / 2, 0, terrainSize / 2);

        // Rotation around centre
        transform.RotateAround (position, Vector3.forward, speed * Time.deltaTime);

    }

    public Color GetColor () {
        return color;
    }

    public Vector3 GetWorldPosition () {
        return transform.position;
    }
}