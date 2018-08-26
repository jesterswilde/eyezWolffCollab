using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSelfDestroy : MonoBehaviour {

	ParticleSystem particle;

	void Start()
	{
		particle = GetComponent<ParticleSystem> ();
	}

	void Update()
	{

		if ( particle!= null) 
		{
			if(!particle.isEmitting)
			{
				Destroy (gameObject);
			}
		}
	}
}
