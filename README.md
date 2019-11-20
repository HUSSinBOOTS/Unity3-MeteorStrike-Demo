# Unity3-MeteorStrike-Demo

1. Start a new 3D Project
2. Choose Import Assets > Space Shooter tutorial
3. Download the assets.
4. Create project from scratch using the assets provided
5. Save the scene. 
6. Create a folder named Scenes. Save the scene and name it `Main`
7. Change the resolution to have a portrait feel of an arcade game (6:10).
8. ----------- The Payer Game Object ----------
9. In the scene view, drag and drop the vehicle player from the Models folder
10. Rename player ship to `Player`.
11. We want the player ship to be at origin. Reset the transformation.
12. Explain  the concepts of "Mesh Filter" and "Mesh Renderer":
	a. Mesh filter to hold mesh model
	b. Mesh renderer to render the filter in the game
13. To use physics, we need to add a rigid body on the `Player`. `Add component  > Rigidbody`
14. Deselect Use Gravity so that the object doesn't fall.
15. Add a capsule collider (2 spheres and a space in between them). This is a cage that will detect collisions
16. Reduce the radius and height to make it fit the model (about 0.85 radius). This is great, but the collider does not look so accurate. A more accurate representation would be a `Mesh Collider`. Mesh colliders is where we supply the mesh collision ourselves.
17. Remove the `Capsule Collider` component from the `Player`.
18. On the `Player` add a `Mesh Collider` component.
19. In `Assets>Models` there is  the `vehicle_playerShip_collider` mesh provided . Drag it and drop it as the `Mesh` attribute of the `Player` `Mesh Collider` component.
20. Turn off the renderer to show the mesh collider around the object and how much more accurate it is.
21. In this game, we don't need to detect full physics collisions (our `Player` won't respond to physics). We simply need our collisions to trigger an action. 
22. Select `Is Trigger` on the `Player` `Mesh Collider`  to make this a trigger collider.
23. In `Prefabs>VFX>Engines>`, attachthe `engines_player`  to the `Player` object in the hierarchy.
24. These are particle effects. Adjust them to fit the tail of the `Player` game object.
25. ----------- Camera and Lights ----------
26. We want this to be a top-down game
27. Manipulate the camera by resetting the camera transform
28. Lift the camera up in the Y axis 
29. Set the X rotation to 90 degrees
30. OR, we can simply use `GameObject > Align With View` option while `Main Camera` is selected, and while looking at the `Player` from above.
31. Projection mode: orthographic (what is this for?)
32. Set the orthographic value to 10
33. Ship should start at the bottom of the screen
34. Drag the main camera Z axis back until the until `Player` is at the bottom of the screen.
35. Change the `Main Camera` `Background` from `Skybox` to a `Solid Color` (we want a background color).
36. Pick the color Black.
37. We will create a 3 point light system
38. In the project Hierarchy, select right click and  `Create > Directional Light`. Rename it to `Main Light`. Reset it's position. Light depends on the rotation, not position.
39.  Reset the rotation. Tilt the main light down so that it's on the ship (about X20). While in game mode,  the light X transform until it looks good basically (which is at about X20).
40. We need a fill light to fill shadows on the other side. Duplicate `Main Light` from the Hierarchy (CMD + D) and rename it to Fill Light.
41. Light the other side of the ship. (Rotation at X10 Y50). 
42. Reduce the intensity (to 0.4), change the color (to blue or red or something POPPING).
43. Group up the lights under a new empty game object named `Lights`. 
44. -----------Background to the Game----------
45. Create a Quad. `Create > Quad` from the Hierarchy. Reset its transformation.
46. It's oriented in a way that's not facing the main camera.  Set the X rotation to 90 degrees. Remove the Mesh Collider on the quad. We won't be using it. Rename it to `Background` 
47. In `Assets > Textures`, drag and drop `tile_nebula_green_dff` onto the `Background` quad.  We'll use that to add texture on the `Background` game object. 
48. Rescale the background. X15 Y35
49. The background is washed out. On the `Background` Shader, change the Shader of the texture.  Select `Unlit > Texture`. The background is now independent of the lighting system we have. It displays the image in its exact form.
50. Move the background down at the y axis (Y-10)
------------------------------------------------------------------
Meteor Strike Task 1
------------------------------------------------------------------
51. -----------Moving the Player----------
52. Create a new folder to store `Scripts`
53. On the `Player` game object in the Hierarchy, create a new `PlayerController` script. 
54. Save it in the scripts folder
55. Open the script
56. Remove `Update` and use Fixed Update instead (look up the difference and show them)
57. Fixed update will be called automatically by unity before each physics step
```
    //Components
    Rigidbody rb;
    // Use this for initialization
    void Start () {
	rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate(){
	move();
	clampPosition();
    }

    void move(){
	float moveHorizontal = Input.GetAxis ("Horizontal");
	float moveVertical = Input.GetAxis ("Vertical");
	Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
	rb.velocity = movement * speed;
    }
```
58. The axis Horizontal and Vertical are preset in the input manager (show that to them).
59. Great, but now if we play the game, the ship is slow. Let's move it faster by adding speed
```
	//Components 
	...

	//SerializeFields
	[SerializeField] private float speed;

	   void Start () {
			...

	    }

	...
	 rb.velocity = movement * speed;
```
60. Now we have a problem. The player ship can leave the area. We need to lock the ship into position.
61. Google C# `Mathf`.  It has a bunch of methods. We need a method called `Clamp`. It basically sets boundaries.
62.  What values to set for xMinMax and zMinMax? Play the game and see where the ship stops

```
	    //SerializeFields
	    [SerializeField] private float speed;
	    [SerializeField] private float xMin, xMax, zMin, zMax;

	    void clampPosition(){
		float clampX = Mathf.Clamp(rb.position.x, xMin, xMax);
		float clampZ = Mathf.Clamp(rb.position.z, zMin, zMax);
		rb.position = new Vector3(clampX, 0.0f, clampZ);
	    }
```

63. Call `clampPosition` in the `FixedUpdate()` method.
64. When it comes to the Z min max, back up a little bit to give room for the comets to come flying in
65. Create a `tilt()` method. Call it in the `FixedUpdate()` method. Quaternion.Euler returns a rotation.

```
//SerializeFields
    [SerializeField] private float tiltValue;

...

    void tilt(){
	rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tiltValue);
    }
```
------------------------------------------------------------------
Meteor Strike Task 2
------------------------------------------------------------------
66. Set the tilt value from the inspector now.
67. ----------- 06 Creating Shots----------
68. Deactivate the player game object
69. Create new empty game object, and rename it to `Bolt`. This will be the parent for our shot objects.
70. Why the bolts in an empty game object? It allows us to use the SAME logic, and swap out the visual effects whenever we wanted to.
71. Reset the transform of `Bolt`.
72. Create a Quad and rename it to `VFX` (Visual Effects) and reset its transform.
73. In the Hierarchy, Make`VFX` as a child of `Bolt`. 
74. Change the rotation of the `Quad` to X90 Y0 Z0
75. Open `Assets/Textures`. Find `fx_lazer_orange_dff`. We will create the material for that ourselves. 
76. With the `Assets/Material` folder selected, create a new material `Right Click > Create > Material`. 
77. Rename it to `fx_orange_bolt`. Expand the material inspector. Change the Shader to `Mobile/Particles/Additive`. Finally, drag and drop the `fx_laser_orange` onto the texture of `fx_orange_bolt` material.
78. Now, drag and drop `fx_bolt_orange` onto the the `VFX` quad.
79. `Add component > Physics> Rigidbody` on the `Bolt` game object. Uncheck "use gravity".
80. Remove mesh collider from `VFX`. Add a capsule collider to it. (capsule is two spheres and the space in between).
81. Adjust the capsule so that it fits around the bolt. Change the "direction" to "Z-axis". 0.03 radius. 0.5 height.
82. Set `Is Trigger` on the capsule collider.
83. Add a new script to `Bolt` and name it `Mover`.
```
public class Mover : MonoBehaviour {
    //SerializeFields
    [SerializeField] private float speed;
    //Components
    Rigidbody rb;
    // Use this for initialization
    void Start () {
	rb = GetComponent<Rigidbody>();
	rb.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update () {

    }
}
```
84. Set the speed from the inspector and play the game. The bullet now moves.
85. Prefab the bolt by dragging it and dropping it inside the `Assets/Prefabs` folder.
86. Delete `Bolt` form the Hierarchy as we only want it when the player shoots.
87. ----------07 Shooting Shots----------
88. Reactivate the `Player` game object.
89. Edit the PlayerController.cs script. Add the following SerializeFields
```
    //SerializeFields
	...
    [SerializeField] private GameObject shot;
    [SerializeField] private Transform shotSpawn;
    [SerializeField] private float fireRate;

    //Variables
    private float nextFire;



```
90. Go back to Unity. 
91. Create new empty game object that will act as a virtual spawn point for our shot. Name it `Shot Spawn` and attach it to the `Player` game object. Drag it so that it is in front of the ship. Drag and drop the `Bolt` prefab into the `Shot Spawn`. Make sure that the instance `Bolt` is at origin (reset transform), and that `Shot Spawn` still aligns in front of the ship.  
92. Once all aligned, remove the bolt from shot spawn.
93. Click on the `Player` game object, and start filling out the SerializeFields. 
	a. "Shot" gets the `Bolt` prefab
	b. "Shot Spawn" gets the `Shot Spawn` empty game object.
	c. Start with a 0.5 fire rate. Adjust it later when trying the feature.
94. Now, let us use `Instantiate` to shoot bullets on clicking. In `PlayerController.cs`, create a new script:
	```
	    void fire(){
		if(Input.GetButton("Fire1") && Time.time > nextFire){
		    nextFire = Time.time + fireRate;
		    Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
		}
	    }
	```
95. Call this script in `Update` and not `FixedUpdate` as it does not involve any physics. 
96. Try the game. If the bullets are slow, change the `Bolt` prefab speed. Adjust `Player` fire rate so that it is appropriate (about 0.1 was good). 
97. We can shoot, but we are filling our scene with endless instances. We need to create a boundary.
98. ----------08 Boundary----------
99. Create a cube in the hierarchy. Rename it to `Boundary` and reset its transformation. In the box collider component, select `Is Trigger`
100. Adjust the scale of the `Boundary` so that it is placed evenly around the game area. Turn off the mesh renderer. 
101. Attach a script to `Boundary` and rename it destroy by boundary. 
102. Show the`OnTriggerExit` from the Unity documentation .
103. Use it in the `DestroyByBoundary` script. 
```
    void OnTriggerExit(Collider other){
	Destroy(other.gameObject);
    }
```
104. Test the game and show that the bolt clones are now being destroyed. 
------------------------------------------------------------------
Meteor Strike Task 3
------------------------------------------------------------------
105. ----------09 Creating Hazards----------
106. Create new empty game object. Name it `Asteroid`
107. Move it somewhere ahead of the player. In `Assets/Models`, Choose any asteroid, then drag it onto the `Asteroid` game object.
108. Reset the transform of the child asteroid object
109. Select parent the  `Asteroid`, and add a Rigidbody to it. Deselect "Use Gravity". Add a capsule collider and adjust the radius of it. 
110. With `Asteroid` selected, add a new Script. Name it `RandomRotator.cs`
```
    //SerializeFields
    [SerializeField] private float tumble;
    //Components
    Rigidbody rb;
    // Use this for initialization
    void Start () {
    rb = GetComponent<Rigidbody>();
    rb.angularVelocity = Random.insideUnitSphere * tumble;  
    }
```
111. Set the tumble value from the inspector. Play the game to show that the Asteroid now has a tumble. 
112. What happens if we shoot the Asteroid? Nothing so far. 
113. Add a new script onto the `Asteroid` and name it `DestroyByContact.cs` . We would like to destroy both the bullet and the asteroid. In `DestroyByContact.cs`: 
```
    void OnTriggerEnter(Collider other){
	Destroy(other.gameObject);
	Destroy(gameObject);
    }
```

114.  It does not matter the order of the game object. Game objects are destroyed at the end of each frame.
115. Play the game. The Asteroid disappears. Something is destroying the asteroid. Print (other.name)
```
    void OnTriggerEnter(Collider other){
	print(other.name);
	Destroy(other.gameObject);
	Destroy(gameObject);
    }
```
 We see that it is the boundary destroying the game object. We need to ignore collision with Boundary on the Asteroid.
116. Select Boundary and add a tag to it. Add a new tag. "Boundary".
117. Back in `DestroyByContact.cs`:
```
    void OnTriggerEnter(Collider other){
	if(other.tag == "Boundary"){
	    return;
	}
	Destroy(other.gameObject);
	Destroy(gameObject);
    }
```
118. ----------10  Explosions----------
119. Inside the `DestroyByContact.cs`: 
```
    //SerializeFields
    [SerializeField] GameObject explosion;

    void OnTriggerEnter(Collider other){
	if(other.tag == "Boundary"){
	    return;
	}
	Instantiate(explosion, transform.position, transform.rotation);
	Destroy(other.gameObject);
	Destroy(gameObject);
    }
```

120. In `Assets/Prefabs/VFX/Explosions`, attach`explosion_asteroid` to the  `Explosion` field of the `DestroyByContact` script of the `Asteroid`.
121. Test it. It should work now. Now, bump the ship into the asteroid. Same explosion. Let us customize one for the players.
122. Add the following to the script: 
```
//SerializeFields
...         
   [SerializeField] GameObject playerExplosion;

...
    void OnTriggerEnter(Collider other){
	if(other.tag == "Boundary"){
	    return;
	}
	if (other.tag == "Player"){
	    Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
	}
	Instantiate(explosion, transform.position, transform.rotation);
	Destroy(other.gameObject);
	Destroy(gameObject);
    }
```
123. Now, tag the player game object to be "Player". Add the  explosion_player to the reference of the script.
124. To make the `Asteroid` move towards the player, attach the `Mover.cs` script to it. Change the speed to -5. That takes care of it. Play the game to show that `Mover` did. Prefab the Asteroid. Delete it from the Hiearchy. Our asteroid is set up and ready to be spawned.
------------------------------------------------------------------
Meteor Strike Task 4
------------------------------------------------------------------
125. ----------11  Game Controller----------
126. Create new empty game object and rename it to `GameController`. Reset the transform. Tag it as `GameController`. Add a script to it and name it `GameController`. This script will be in charge of spawning hazards in our game.
```
    //SerializeFields
    [SerializeField] GameObject hazard;
    [SerializeField] Vector3 spawnValues;
```
127. In `GameController` game object, drag and drop the `Asteroid` prefab onto the Hazard slot. You have to eyeball the X and Z spawn values to fill them up.
128. In `GameController.cs`, create the `SpawnWaves()` method, then call it in`Start()`:
```
    void SpawnWaves(){
	Vector3 spawnPosition = new Vector3 (Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
	Quaternion spawnRotation = Quaternion.identity;
	Instantiate(hazard, spawnPosition, spawnRotation);
    }
```
129. `Quaternion.identity` means no rotation.
130. Play the game several times, showing that the asteroid now spawns at different points. 
131. ----------12  Spawning Waves ----------
132. We would have to cycle through a loop.
```
    void SpawnWaves(){
	for (int i=0; i<hazardCount; i++){
	Vector3 spawnPosition = new Vector3 (Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
	Quaternion spawnRotation = Quaternion.identity;
	Instantiate(hazard, spawnPosition, spawnRotation);
	}
    }
```
133. Select `GameController`, and change the HazardCount to 10. We need to wait between hazards. Add WaitForSeconds at the end.
```
    //SerializeFields
    [SerializeField] private int spawnWait;
...     
	IEnumerator SpawnWaves(){
	for (int i=0; i<hazardCount; i++){
	Vector3 spawnPosition = new Vector3 (Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
	Quaternion spawnRotation = Quaternion.identity;
	Instantiate(hazard, spawnPosition, spawnRotation);
	yield return new WaitForSeconds(spawnWait);
	}
    }
```
134. Play the game. Nothing starts. Since we are using WaitForSeconds, we need to use StartCoroutine when calling `SpawnWaves()`
```
    // Use this for initialization
    void Start () {
	StartCoroutine(SpawnWaves());
    }
```
135. Play the game. Great but now there is nothing to do once all asteroids are destroyed. Also, asteroids spawn instantly. Let us add a while loop.

136. Add a while loop
```
    //SerializeFields
    [SerializeField] private int startWait;
    [SerializeField] private int waveWait;

     IEnumerator SpawnWaves(){
	yield return new WaitForSeconds(startWait);
	while(true){
		for (int i=0; i<hazardCount; i++){
		Vector3 spawnPosition = new Vector3 (Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
		Quaternion spawnRotation = Quaternion.identity;
		Instantiate(hazard, spawnPosition, spawnRotation);
		yield return new WaitForSeconds(spawnWait);
	}
	    yield return new WaitForSeconds(waveWait);
	}
    }
```

137. Show the hierarchy while playing the game. The explosion clones are not being destroyed. Create new Script. DestroyByTime.
```
    //SerializeFields
    [SerializeField] private float lifetime;
    // Use this for initialization
    void Start () {
	Destroy(gameObject, lifetime);
    }
```
138. Attach the `DestroyByTime.cs` script on all explosions. Give a lifetime of 2.
------------------------------------------------------------------
Meteor Strike Task 5
------------------------------------------------------------------
139. ----------13  Audio ----------
140. Audio folder in assets with Audio Clips 
141. Drag explosion asteroid clip onto the explosion asteroid prefab. Play on Awake should be checked. Do the same for the player explosion.
142. Drag and drop `weapon_player` audio onto the `Player` game object. 
143. In `PlayerController.cs`:
```
    //Components
	...
    AudioSource audiosource;

    void Start () {
	audiosource = GetComponent<AudioSource>();
	...

    }

    void fire(){
	if(Input.GetButton("Fire1") && Time.time > nextFire){
	    audiosource.Play();
	    nextFire = Time.time + fireRate;
	    Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
	}
    }
```

Drag and drop the `music_background` onto the `GameController` object to play background music.

----------13  Scoreboard ----------

In `GameController.cs`:

```
	[SerializeField] Text scoreText;

	//Variables
	private int score;
	
	...
	
	public void AddScore(int newScoreValue){
		score += newScoreValue;
		scoreText.text = "Score: " + score;
	}
```
Make the method public so that we can reference and use it in another script.
In the game Hierarchy, create a Text UI. Place the text in an appropriate place on the canvas. Drag and drop the Text object onto the SerializeField in the `GameController.cs`.

In `DestroyByContact`.cs:

```
 private GameController gc;
 
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gc = gameControllerObject.GetComponent<GameController>();
    }
 ...
 
     void OnTriggerEnter(Collider other)
    {
        if (...)
        {
            ...;
        }
        if (...)
        {
            ...
        }
        gc.AddScore(10);
	...
	...
	...
    }
```

------------------------------------------------------------------
Meteor Strike Task 6
------------------------------------------------------------------
