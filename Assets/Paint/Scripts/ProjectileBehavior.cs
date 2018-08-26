using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour {
	public ParticleSystem burstEffect;
	public float maxDiameter;
	public Color projectileColor = new Color (1, 0, 0, 1);
	public int howManyToSpawn = 5; 
	public float splashVelocity = 5; 

	void Start()
	{
		gameObject.GetComponent<Renderer> ().material.color = projectileColor;
	}

	void OnTriggerEnter(Collider other)
	{
		ParticleSystem burstVFX = Instantiate (burstEffect, transform.position, Quaternion.identity) as ParticleSystem;
		var burstSetting = burstVFX.main;
		burstSetting.startColor = projectileColor;
		burstVFX.Play ();

		RaycastHit hit;
		Vector3 velocity = GetComponent<Rigidbody> ().velocity.normalized;
		Vector3 rayDir = Vector3.Lerp (velocity, Random.onUnitSphere, 0.49f); 
		float hardCodedDistance = 0.5f; 
		Vector3 origin = transform.position - velocity * hardCodedDistance; 
		//Vector3
		if (Physics.Raycast (origin, rayDir, out hit, 100)) 
		{
			Vector2 hitCoord; 
			if (hit.collider is MeshCollider) {
				hitCoord = hit.textureCoord2; 
			} else {
				hitCoord = hit.textureCoord;
			}
			CanvasBehavior canvas = hit.collider.gameObject.GetComponent<CanvasBehavior> ();
			if (canvas != null) {
				canvas.ProjectileDraw (ProjectileManager.SplashAlphaInfo, projectileColor, hitCoord);
			}
			SpawnChildren (hit, velocity);  
		}
		Destroy (gameObject);
	}

	void SpawnChildren(RaycastHit hit, Vector3 velocity)
	{
		for (int i = 0; i < howManyToSpawn; i++) {
			Vector3 newDir = Vector3.Lerp (Random.onUnitSphere, Vector3.Reflect (velocity, hit.normal), 0.3f); 
			GameObject go = Instantiate (gameObject); 
			go.transform.position += newDir;
			go.GetComponent<Rigidbody> ().velocity = newDir * 5; 
			go.GetComponent<ProjectileBehavior> ().howManyToSpawn = howManyToSpawn -1; 
		}
	}
}
