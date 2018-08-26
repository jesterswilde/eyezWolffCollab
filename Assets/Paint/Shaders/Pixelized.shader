Shader "Custom/Pixelized" {

	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_PixelSize ("Pixel Size", int) = 5
		_Percision ("Percision", int) = 500
	}
	
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Geometry+5"}
		LOD 200
		
		CGINCLUDE
		
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		int _PixelSize; 
		int _Percision;
	
		
		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			float4 vertex : SV_POSITION;
		};

		v2f vert (appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = v.uv;
			return o;
		}

		float4 frag (v2f IN) : COLOR {
			float x = IN.uv.x - fmod(IN.uv.x * _Percision, _PixelSize) / _Percision; 
			float y = IN.uv.y - fmod(IN.uv.y * _Percision, _PixelSize) / _Percision; 
			return tex2D(_MainTex, float2(x,y));

		}
		ENDCG
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
		}
	FallBack "Diffuse"
}