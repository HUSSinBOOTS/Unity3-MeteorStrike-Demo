using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerExit(Collider other){
		Destroy(other.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
