using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEmitter : MonoBehaviour {
	public Transform launcherPos;
	public float speed;
	public Object projectilePrefab;

	[SerializeField]
	Transform canvas;

	void Update()
	{
		GameObject projectile;
		if (Input.GetKeyDown ("space")) 
		{
			projectile = Instantiate(projectilePrefab, transform.position + launcherPos.transform.forward, Quaternion.identity) as GameObject; 
			projectile.GetComponent<Rigidbody> ().velocity = launcherPos.transform.forward * speed;
		}
	}
}
