using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBehavior : MonoBehaviour {
    int textureHeight = 256;
    int textureWidth = 256;
    public Color color = new Color(0,0,0,0);
    Texture2D painting;
	Renderer rend;
    Material material;

	void Start () {
        rend = GetComponent<Renderer>();
        material = rend.material;
		painting = new Texture2D(textureWidth, textureHeight);
        for(int x = 0; x < textureWidth; x++)
        {
            for(int y = 0; y < textureHeight; y++)
            {
                painting.SetPixel(x, y, color);
            }
        }
        painting.Apply();
		material.SetTexture("_MainTex", painting);
	}
	
    public void BrushDraw(Texture2D splashTex, Vector2 texCoord)
    {
        int x = (int)(texCoord.x * textureWidth - splashTex.width / 2);
        int y = (int)(texCoord.y * textureHeight - splashTex.height / 2);
        for(int i = 0; i < splashTex.width; i++)
        {
            for(int j = 0; j < splashTex.height; j++)
            {
                int newX = x + i;
                int newY = y + j;
                Color current = painting.GetPixel(newX, newY);
				Color target = splashTex.GetPixel (i, j);
                float alpha = target.a;
               if (alpha > 0)
                {
					Color output = Color.Lerp(current, target, target.a);
					painting.SetPixel(newX, newY, output);
                }
            }
        }
        painting.Apply();
    }

	public void ProjectileDraw(float[,] splashMask, Color targetColor, Vector2 texcoord)
	{
		MyProjectileDraw (splashMask, targetColor, texcoord);
	}

	private void MyProjectileDraw(float[,] splashMask, Color targetColor, Vector2 texCoord)
	{
		int splashWidth = splashMask.GetLength (0);
		int splashHeight = splashMask.GetLength (1);
		int rectX = (int)(texCoord.x * textureWidth - splashWidth / 2);
		int rectY = (int)(texCoord.y * textureHeight - splashHeight / 2) ;
		int right = textureWidth - 1;
		int bottom = textureHeight - 1;
//		Debug.Log ("textCoord " + texCoord + "rectX " + rectX + "rextY " + rectY);
		int a, b;
		int boundX = Mathf.Min (rectX + splashWidth, right);
		int boundY = Mathf.Min (rectY + splashHeight, bottom);

		if (boundX == right) 
		{
			a = splashWidth / 2;
		} else {
			a = 0;
		}
		if (boundY == bottom) 
		{
			b = splashHeight / 2;
		} else 
		{
			b = 0;
		}

		int x = Mathf.Max(rectX - a, 0);
		int y = Mathf.Max(rectY - b, 0);
		int columnsForX = Mathf.Min (x + splashWidth, right) - x;
		int rowsForY = Mathf.Min (y + splashHeight, bottom) - y;

		Color[] pixels = painting.GetPixels (x, y, columnsForX, rowsForY);
		int counter = 0;
		for (int i = 0; i < columnsForX; i++) 
		{
			for (int j = 0; j < rowsForY; j++) 
			{
				Color currentPixel = pixels[counter];
				float splashAlpha = splashMask [i, j];
				Color outputColor = Color.Lerp (currentPixel, targetColor, splashAlpha);
				pixels [counter] = outputColor;
				counter++;
			}
		}
		painting.SetPixels (x, y, columnsForX, rowsForY, pixels);
		painting.Apply ();
	}
}
