using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TerrainCollider))]
public class DiamondSquareTerrain : MonoBehaviour {

    // Container for heights of a terrain
    private TerrainData terrainData;

    // Size of terrain
    private int size;

    // Maximum array size
    private int maxSize;

    // 2D terrain array
    private float[,] heights;

    // Variable determining roughness of heights
    private float roughness = 0.8f;

    // Use this for initialization
    public void Start() {

        // Get terrain data
        terrainData = this.transform.GetComponent<TerrainCollider>().terrainData;

        // Position terrain at origin (0, 0, 0)
        this.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

        // Get terrain sizew
        size = terrainData.heightmapWidth;
        maxSize = size - 1;

        // Initialise terrain
        InitialiseTerrain();

        // Excecute Diamond Square
        DiamondSquare();
    }

    private void InitialiseTerrain() {
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
        terrainData.SetHeights(0, 0, heights);
    }

    private void DiamondSquare() {
        int stepSize = size - 1;
        float range = 0.5f;

        heights = new float[size, size];

        while (stepSize > 1) {

            // Diamond step
            DiamondStep(stepSize, range);

            // Square step
            SqaureStep(stepSize, range);

            // Lower the random value range
            range -= range * 0.5f * roughness;

            // Half step size
            stepSize /= 2;
        }

        // Update terrain heights
        terrainData.SetHeights(0, 0, heights);
    }

    private void DiamondStep(int stepSize, float range) {
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

    private void SqaureStep(int stepSize, float range) {
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

    public float getSize() {
        return size;
    }

}