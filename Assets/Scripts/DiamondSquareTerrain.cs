using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// When attached to a Terrain GameObject, it can be used to randomise heights
/// using the Diamond-Square algorithm taught in lectures.
/// </summary>
public class DiamondSquareTerrain : MonoBehaviour {

    // Container for heights of a terrain
    private TerrainData terrainData;

    // Size of terrain
    private int size;

    // Maximum array size
    private int maxSize;

    // 2D terrain array storing heights
    private float[, ] heights;

    // Variable determining roughness of heights
    public float roughness;

    // Terrain material
    private Material material;

    /// <summary>
    /// Used for initialization.
    /// </summary>
    private void Start () {
        // Get active terrain
        Terrain terrain = Terrain.activeTerrain;

        // Get Terrain material
        material = terrain.materialTemplate;

        // Get terrain data
        terrainData = terrain.terrainData;

        // Position terrain at origin (0, 0, 0)
        this.transform.position = new Vector3 (0.0f, 0.0f, 0.0f);

        // Get terrain size
        size = terrainData.heightmapWidth;
        maxSize = size - 1;

        // Initialise terrain
        InitialiseTerrain ();

        // Excecute Diamond Square algorithm
        DiamondSquare ();

        // Calculate maximum height in terrain
        float maxHeight = GetMaxHeight ();

        // Intialise landscape heights
        float dirtHeight = (float) 0.1 * maxHeight;
        float grassHeight = (float) 0.25 * maxHeight;
        float rockHeight = (float) 0.5 * maxHeight;

        // Pass heights to shader
        material.SetFloat ("_DirtHeight", dirtHeight);
        material.SetFloat ("_GrassHeight", grassHeight);
        material.SetFloat ("_RockHeight", rockHeight);

    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update () {
        // Get sun object
        GameObject sun = GameObject.Find ("Sphere");
        SunRotation sunRotation = sun.GetComponent<SunRotation> ();

        // Pass color of sun and world position to shader
        material.SetColor ("_PointLightColor", sunRotation.GetColor ());
        material.SetVector ("_PointLightPosition", sunRotation.GetWorldPosition ());
    }

    /// <summary>
    /// Initialises terrain corners with random values.
    /// </summary>
    private void InitialiseTerrain () {
        heights = new float[size, size];

        // Bottom left
        heights[0, 0] = Random.value;

        // Bottom right
        heights[maxSize, 0] = Random.value;

        // Top left
        heights[0, maxSize] = Random.value;

        // Top right
        heights[maxSize, maxSize] = Random.value;

        // Update terrain heights
        terrainData.SetHeights (0, 0, heights);
    }

    /// <summary>
    /// Heart of Diamond Square algorithm. 
    /// </summary>
    private void DiamondSquare () {
        int stepSize = size - 1;
        float range = 0.5f;

        heights = new float[size, size];

        // Keep executing square and diamond steps until terrain is finalised
        while (stepSize > 1) {

            // Diamond step
            DiamondStep (stepSize, range);

            // Square step
            SqaureStep (stepSize, range);

            // Lower the random value range
            range -= range * (roughness - 0.1f) * roughness;

            // Half step size
            stepSize /= 2;
        }

        // Update terrain heights
        terrainData.SetHeights (0, 0, heights);
    }

    /// <summary>
    /// Perform Diamond step of Diamond Square algorithm. Updates heights array
    /// with calculated averages.
    /// </summary>
    /// <param name="stepSize"></param>
    /// <param name="range"></param>
    private void DiamondStep (int stepSize, float range) {
        int midPoint = stepSize / 2;

        // Traverse x, y point heights
        for (int x = 0; x < maxSize; x += stepSize) {
            for (int y = 0; y < maxSize; y += stepSize) {

                // Get accumulated corners
                float accum = heights[x, y] +
                    heights[x + stepSize, y] +
                    heights[x, y + stepSize] +
                    heights[x + stepSize, y + stepSize];

                // Calculate averages
                float average = accum / 4.0f;

                // Offset by random value
                average += (Random.value * (range * 2.0f)) - range;

                // Set height to calculated average
                heights[x + midPoint, y + midPoint] = average;
            }
        }
    }

    /// <summary>
    /// Perform Diamond step of Diamond Square algorithm. Updates heights array
    /// with calculated averages
    /// </summary>
    /// <param name="stepSize"></param>
    /// <param name="range"></param>
    private void SqaureStep (int stepSize, float range) {
        int midPoint = stepSize / 2;

        // Traverse x, y points
        for (int x = 0; x < maxSize; x += midPoint) {
            for (int y = (x + midPoint) % stepSize; y < maxSize; y += stepSize) {

                // Get accumulated corners
                float accum = heights[(x - midPoint + maxSize) % maxSize, y] +
                    heights[(x + midPoint) % maxSize, y] +
                    heights[x, (y + midPoint) % maxSize] +
                    heights[x, (y - midPoint + maxSize) % maxSize];

                // Calculate averages
                float average = accum / 4.0f;

                // Offset by random value
                average += (Random.value * (range * 2.0f)) - range;

                // Set height to calculated average
                heights[x, y] = average;

                // Set height to opposite edge is this is an edge piece
                if (x == 0) {
                    heights[maxSize, y] = average;
                }

                if (y == 0) {
                    heights[x, maxSize] = average;
                }
            }
        }
    }

    /// <summary>
    /// Get Max height inside terrain.
    /// </summary>
    /// <returns>Returns the highest point in terrain array</returns>
    private float GetMaxHeight () {

        // Set maximum height as lowest possible number
        float currMaxHeight = float.MinValue;

        // Traverse x and y points
        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {

                // Get height in world perspective
                float height = terrainData.GetHeight (x, y);

                // Replace current max height
                if (height > currMaxHeight) {
                    currMaxHeight = height;
                }
            }
        }

        return currMaxHeight;
    }

}