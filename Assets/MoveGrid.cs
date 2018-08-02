using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoveGrid : MonoBehaviour {

	[SerializeField]
	int height; 
	[SerializeField]
	int width; 
	[SerializeField]
	UnityEngine.Object prefab; 

	List<Mover> movers = new List<Mover>(); 

	[SerializeField]
	float threshold = 4f; 
	float counter = 0; 


	// Use this for initialization
	void Start () {
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				GameObject go = Instantiate (prefab, new Vector3 (x * 2, y * 2, 0), Quaternion.identity) as GameObject; 
				movers.Add (go.GetComponent<Mover>()); 
			}
		}


	}

	void Update(){
		counter += Time.deltaTime;
		if (counter > threshold) {
			counter = 0; 
			Swap (); 
		}
	}

	void Swap(){
		List<Transform> destinations = movers.Select ((move) => move.transform).ToList(); 
		for (int i = 0; i < destinations.Count; i++) {
			Transform temp = destinations [i]; 
			int newIndex = Random.Range (0, destinations.Count); 
			destinations [i] = destinations [newIndex];
			destinations [newIndex] = temp; 
		}
		for (int i = 0; i < destinations.Count; i++) {
			movers [i].MoveTo (destinations [i].position); 
		}

//
//		while (destinations.Count > 0) {
//			movers [index].MoveTo (destinations [0].position); 
//			index++; 
//			destinations.RemoveAt (0); 
//		}
		//[] 
	}

}
