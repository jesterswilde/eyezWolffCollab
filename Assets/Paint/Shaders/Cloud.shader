Shader "Unlit/Cloud"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Speed("Speed", Float) = 1
		_Amp("Amplitute", Float) = 1
		_Distance("Distance", Float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"


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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Speed;
			float _Amp;
			float _Distance;
			
			v2f vert (appdata v)
			{
				v2f o;
				float4 worldSpaceVertex = mul(unity_ObjectToWorld, v.vertex);
				worldSpaceVertex.x += sin(_Time.y * _Speed+ worldSpaceVertex.y * _Amp) * _Distance;
				v.vertex = mul(unity_WorldToObject, worldSpaceVertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
