using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotation : MonoBehaviour {

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

        // Get size of terrain
        GameObject terrainObject = GameObject.Find ("Terrain");
        DiamondSquareTerrain terrain = terrainObject.GetComponent<DiamondSquareTerrain> ();
        float terrainSize = terrain.GetSize ();

    }
}