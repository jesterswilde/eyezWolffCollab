using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessing : MonoBehaviour {

	[SerializeField]
	Material lowPixels;
	[SerializeField]
	int pixelSize = 5;
	[SerializeField]
	int percision = 500;

	void Update()
	{
		lowPixels.SetInt ("_PixelSize", pixelSize);
		lowPixels.SetInt ("_Percision", percision);
	}
	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		Graphics.Blit (src, dst, lowPixels);
	}
}
