using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// When attached to a Terrain GameObject, it can be used to randomise heights
/// using the Diamond-Square algorithm taught in lectures.
/// </summary>
[RequireComponent (typeof (TerrainData))]
[RequireComponent (typeof (Terrain))]
public class DiamondSquareTerrain : MonoBehaviour {

    // Container for heights of a terrain
    private TerrainData terrainData;

    // Terrain object
    private Terrain terrain;

    // Size of terrain
    private int size;

    // Maximum array size
    private int maxSize;

    // 2D terrain array storing heights
    private float[, ] heights;

    // Variable determining roughness of heights
    public float roughness = 0.8f;

    // Maximum height in terrain
    private float maxHeight;

    // Texture indices
    private static int DIRT = 0;
    private static int GRASS = 1;
    private static int ROCK = 2;
    private static int SNOW = 3;

    // Heights for textures
    private float dirtHeight;
    private float grassHeight;
    private float rockHeight;

    // Terrain material
    public Material material;

    /// <summary>
    /// Used for initialization.
    /// </summary>
    private void Start () {
        // Get active terrain
        terrain = Terrain.activeTerrain;

        // Get Terrain material
        material = terrain.materialTemplate;

        // Get terrain data
        // Also attach a TerrainCollider for collision detection
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
        maxHeight = terrainData.bounds.max.y;

        // Intialise landscape heights
        dirtHeight = (float) 0.1 * maxHeight;
        grassHeight = (float) 0.25 * maxHeight;
        rockHeight = (float) 0.5 * maxHeight;

        // Pass heights to shader
        material.SetFloat ("_DirtHeight", dirtHeight);
        material.SetFloat ("_GrassHeight", grassHeight);
        material.SetFloat ("_RockHeight", rockHeight);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update () {
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
            range -= range * Mathf.Pow (roughness, 2);

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
    /// Adds textures to terrain.
    /// </summary>
    private void AddTextures () {

        // Splatmap data is stored internally as a 3d array of floats
        float[, , ] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        // Loop over the points and assign textures
        for (int y = 0; y < terrainData.alphamapHeight; y++) {
            for (int x = 0; x < terrainData.alphamapWidth; x++) {
                float height = terrainData.GetHeight (y, x);

                if (height <= dirtHeight) {
                    splatmapData[x, y, DIRT] = 1.0f;
                } else if (height <= grassHeight && height > dirtHeight) {
                    splatmapData[x, y, GRASS] = 1.0f;
                } else if (height <= rockHeight && height > grassHeight) {
                    splatmapData[x, y, ROCK] = 1.0f;
                } else {
                    splatmapData[x, y, SNOW] = 1.0f;
                }

            }
        }

        // Finally assign the new splatmap to the terrainData:
        terrainData.SetAlphamaps (0, 0, splatmapData);
    }

    /// <summary>
    /// Getter for size of terrain map.
    /// </summary>
    /// <returns>Returns size of map</returns>
    public float GetSize () {
        return size;
    }

}