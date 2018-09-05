using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When attached to a plane object, a plane is inserted underneath your Terrain
/// object and waves are generated.
/// </summary>
[RequireComponent (typeof (Terrain))]
[RequireComponent (typeof (MeshFilter))]
public class WaterPlane : MonoBehaviour {
    // Mesh holding verticies
    public Mesh mesh;

    // Hardcoded mapscale which fits the terrain
    public float mapScale = 51.0f;

    // Plane heights
    private Vector3[] heights;

    // Counter used to update waves
    private float currentTime = 0.0f;

    // Threshold which indicates when new waves should be generated
    public float timeStep = 0.2f;

    // Start and end thresholds for mesh heights
    public float start = -0.1f;
    public float end = 1.0f;

    // Use this for initialization
    private void Start () {

        // Obtain terrain obejct
        GameObject terrainObject = GameObject.Find ("Terrain");
        Terrain terrain = terrainObject.GetComponent<Terrain> ();

        // Get terrains size
        // Square map for just width is fine
        int terrainSize = terrain.terrainData.heightmapWidth;

        // Position plane in center of terrain
        this.transform.position = new Vector3 (terrainSize / 2, 1.0f, terrainSize / 2);

        // Enlarge plane to fit under terrain
        float xScale = this.transform.localScale.x * mapScale;
        float yScale = this.transform.localScale.y;
        float zScale = this.transform.localScale.z * mapScale;
        this.transform.localScale = new Vector3 (xScale, yScale, zScale);

        // Extract mesh from properties
        mesh = this.gameObject.GetComponent<MeshFilter> ().mesh;
        heights = mesh.vertices;
    }

    /// <summary>
    // Update is called once per frame.
    /// </summary>
    private void Update () {

        // If timestep expired, generate new waves
        if (Time.time - currentTime > timeStep) {
            generateWaves ();
            currentTime = Time.time;
        }

    }

    /// <summary>
    /// Generates waves on plane.
    /// </summary>
    private void generateWaves () {
        Vector3[] vertices = new Vector3[heights.Length];

        // Assign random heights to vertices
        // Creates water bobbing up and down effect
        for (int i = 0; i < vertices.Length; i++) {
            Vector3 vertex = heights[i];

            vertex.y += Random.Range (start, end);
            vertices[i] = vertex;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals ();
        mesh.RecalculateBounds ();
    }
}