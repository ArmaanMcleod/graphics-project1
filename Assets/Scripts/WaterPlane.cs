using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When attached to a plane object, a plane is inserted underneath your Terrain
/// object and waves are generated.
/// </summary>
public class WaterPlane : MonoBehaviour {

    // Hardcoded mapscale which fits the terrain
    public float mapScale = 51.0f;

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
    }
}