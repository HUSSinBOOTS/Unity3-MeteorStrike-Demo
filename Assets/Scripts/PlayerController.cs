using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	
	
	//SerializeFields
	[SerializeField] private float tiltValue;
	[SerializeField] private float speed;
	[SerializeField] private float xMin, xMax, zMin, zMax;
	[SerializeField] private GameObject shot;
	[SerializeField] private Transform shotSpawn;
	[SerializeField] private float fireRate;
	
	//Variables
	private float nextFire;

	//Components
	AudioSource audiosource;
	Rigidbody rb;


	// Use this for initialization
	void Start () {
		audiosource = GetComponent<AudioSource>();
		rb = GetComponent<Rigidbody>();
		
	}
	
	// Update is called once per frame
	void Update(){
		fire();
	}

	void FixedUpdate(){
		move();
		clampPosition();
		tilt();
	}


	void move(){
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rb.velocity = movement * speed;
	}

	void clampPosition(){
		float clampX = Mathf.Clamp(rb.position.x, xMin, xMax);
		float clampZ = Mathf.Clamp(rb.position.z, zMin, zMax);

		rb.position = new Vector3(clampX, 0.0f, clampZ);
	}

	void tilt(){
		rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tiltValue);
	}

	void fire(){
		if(Input.GetButton("Fire1") && Time.time > nextFire){
			audiosource.Play();
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
		}
	}
}
