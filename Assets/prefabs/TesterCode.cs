using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterCode : MonoBehaviour {

	[SerializeField]
	float timeScale;
	[SerializeField]
	float fakeTimeScale = 1; 
	float previousTime; 
	float deltaTime; 

	[SerializeField]
	Transform dependentTrans; 
	[SerializeField]
	Transform independentTrans; 
	[SerializeField]
	float speed; 

	void Start () {
		Time.timeScale = timeScale; 
		Time.fixedDeltaTime = 0.1234f; 
	}
	void FixedUpdate(){
		Debug.Log(Time.fixedDeltaTime); 
	}
	void Update () {
		dependentTrans.position += Vector3.forward * 0.02f * speed; 
		independentTrans.position += Vector3.forward * Time.deltaTime * speed; 
		deltaTime = Time.realtimeSinceStartup - previousTime; 
		previousTime = Time.realtimeSinceStartup;
		
	}
}
