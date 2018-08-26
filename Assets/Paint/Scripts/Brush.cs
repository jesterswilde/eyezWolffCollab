using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{

	public Texture2D splashTexture;
	public Camera cam;
	Vector2 texCoord;

	private void Update ()
	{
		if (Input.GetMouseButton (0)) {
			Cast ();
		}
	}

	void Cast ()
	{
		Ray ray = cam.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			CanvasBehavior canvas = hit.collider.gameObject.GetComponent<CanvasBehavior> ();
			if (canvas != null) {
				texCoord = hit.textureCoord;
				canvas.BrushDraw (splashTexture, texCoord);
			}
            ProcTerrain.SetHeight(texCoord, splashTexture);
		}
	}

}
