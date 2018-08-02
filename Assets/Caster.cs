using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caster : MonoBehaviour {
	
	void Update () {
		if (Input.GetMouseButton (0)) {
			Cast (); 
		}
			
	}

	void Cast(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
		RaycastHit hit; 
		if (Physics.Raycast (ray, out hit)) {
			Map map = hit.collider.gameObject.GetComponent<Map> (); 
			if(map != null){
				map.clickedOnPoint (hit.textureCoord); 
			}
		}
	}
}
