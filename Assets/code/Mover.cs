using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {


	float hardCodedSpeed = 3f; 
	Vector3 destination; 

	public void MoveTo(Vector3 moveTo){
		destination = moveTo; 
	}

	void Awake(){
		destination = transform.position; 
	}
	// Update is called once per frame
	void Update () {
		MoveCube (); 
	}

	void MoveCube(){
		if (destination != transform.position) {
			if (Vector3.Distance (destination, transform.position) < 0.1f) {
				transform.position = destination; 
			} else {
				transform.position = Vector3.Lerp (transform.position, destination, Time.deltaTime * hardCodedSpeed);
			}
		}
	}
}
