COMP30019 Project 1 2018

Diamond Square Terrain Generation:
* Create 2D array holding float heights.
* Randomize corner points of array.
* Complete diamond and square steps.
* Repeated until random fractal landscape is created.
* Chose 0.8 as a roughness factor.

Implementation Choices:
* Used Unity Terrain object instead of mesh to hold terrain heights.
* We represented the terrain textures based on terrain heights(dirt < grass < rock < snow).
* We used Sphere/Terrain colliders to achieve collision detection.
* Inserted plane underneath terrain to hold water. 
* Sphere object with halo used to create sun.

Note:
* Diamond Square algorithm inspired by http://www.playfuljs.com/realistic-terrain-in-130-lines/. 
* Cg/HLSL PhongShader from lab 5 was used to supplement the Phong Illumination model. Modified specular and diffuse constants.
* Parts of Cg/HLSL WaveSahder from lab 4 was used to generate waves.
* Script parameters are customizable through Unity inspector. No need to be modified when running project.
* Due to the randomness of the Diamond square algorithm, sometimes the terrain renders flat. 
* Phong Shading only applied to terrain.