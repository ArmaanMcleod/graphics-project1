using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPlane : MonoBehaviour {
    public Mesh mesh;

    public float mapScale = 51.0f;

    private Vector3[] heights;

    private float currentTime = 0.0f;

    // Use this for initialization
    void Start () {
        GameObject terrainObject = GameObject.Find ("Terrain");
        Terrain terrain = terrainObject.GetComponent<Terrain> ();
        int terrainSize = terrain.terrainData.heightmapWidth;

        this.transform.position = new Vector3 (terrainSize / 2, 1.0f, terrainSize / 2);

        float xScale = this.transform.localScale.x * mapScale;
        float yScale = this.transform.localScale.y;
        float zScale = this.transform.localScale.z * mapScale;
        this.transform.localScale = new Vector3 (xScale, yScale, zScale);

        // Obtain the mesh
        mesh = this.gameObject.GetComponent<MeshFilter> ().mesh;
    }

    // Update is called once per frame
    void Update () {
        if (Time.time - currentTime > 0.3f) {
            generateWaves ();
            currentTime = Time.time;
        }

    }

    private void generateWaves () {
        if (heights == null) {
            heights = mesh.vertices;
        }

        Vector3[] waveVertices = new Vector3[heights.Length];
        for (int i = 0; i < waveVertices.Length; i++) {
            Vector3 vertex = heights[i];
            vertex.y += Random.Range (0.1f, 1.0f);
            waveVertices[i] = vertex;
        }
        mesh.vertices = waveVertices;
        mesh.RecalculateNormals ();
    }
}