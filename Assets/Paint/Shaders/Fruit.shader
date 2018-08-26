Shader "Custom/Fruit" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Main Texture", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		_MeltY("MeltY", Float) = 0.0
		_MeltDist("Melt Distance", Float) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			half _Speed;
			half _Amount;
			half _Distance;

			float _MeltY;

			float4 getNewVertexPosition(float4 objectSpaceVertex, float3 objectSpaceNormal)
			{
				float4 worldSpaceVertex = mul(unity_ObjectToWorld, objectSpaceVertex);
				float4 worldSpaceNormal = mul(unity_ObjectToWorld, float4(objectSpaceNormal, 0));

				float melt = worldSpaceVertex.y - _MeltY;
				//Kisses effect without saturate.
				melt = 1 - saturate(melt);
				worldSpaceVertex.xz += worldSpaceNormal.xz * melt;

				return mul(unity_WorldToObject, worldSpaceVertex);
			}

			v2f vert (appdata v)
			{
				v2f o;
				float4 newVertexPos = getNewVertexPosition(v.vertex, v.normal);
				float4 bitangent = float4(cross(v.normal, v.tangent), 0); 

				float vertOffset = 1;
				float4 UDir = getNewVertexPosition(v.vertex + v.tangent * vertOffset, v.normal);
				float4 VDir = getNewVertexPosition(v.vertex + bitangent * vertOffset, v.normal);

				float4 newTangent = UDir - newVertexPos;
				float4 newBitangent = VDir - newVertexPos;
				o.normal = cross(newTangent, newBitangent);
				o.vertex = UnityObjectToClipPos(newVertexPos);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
//		CGPROGRAM
//
//		#pragma surface surf Standard fullforwardshadows vertex:vert
//
//		#pragma target 3.0
//
//		sampler2D _MainTex;
//
//		struct Input {
//			float2 uv_MainTex;
//		};
//
//		half _Glossiness;
//		half _Metallic;
//		fixed4 _Color;
//		float _MeltY;
//		float _MeltDist;
//
//		float4 getNewVertexPosition(float4 objectSpaceVertex, float3 objectSpaceNormal)
//		{
//			float4 worldSpaceVertex = mul(unity_ObjectToWorld, objectSpaceVertex);
//			float4 worldSpaceNormal = mul(unity_ObjectToWorld, float4(objectSpaceNormal, 0));
//
//			float melt = worldSpaceVertex.y - _MeltY;
//			//Kisses effect without saturate.
//			melt = 1 - saturate(melt);
//			worldSpaceVertex.xz += worldSpaceNormal.xz * melt;
//
//			return mul(unity_WorldToObject, worldSpaceVertex);
//		}
//
//		void vert (inout appdata_full v)
//		{
//			float4 newVertexPos = getNewVertexPosition(v.vertex, v.normal);
//
//			float4 bitangent = float4(cross(v.normal, v.tangent), 0); 
//
//			float vertOffset = 1;
//			float4 UDir = getNewVertexPosition(v.vertex + v.tangent * vertOffset, v.normal);
//			float4 VDir = getNewVertexPosition(v.vertex + bitangent * vertOffset, v.normal);
//
//			float4 newTangent = UDir - newVertexPos;
//			float4 newBitangent = VDir - newVertexPos;
//			v.normal = cross(newTangent, newBitangent);
//			v.vertex = newVertexPos;
//		}
//
//		void surf (Input IN, inout SurfaceOutputStandard o) {
//			// Albedo comes from a texture tinted by color
//			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
//			o.Albedo = c.rgb;
//			o.Metallic = _Metallic;
//			o.Smoothness = _Glossiness;
//			o.Alpha = c.a;
//		}
//		ENDCG
//	}
//	FallBack "Diffuse"
}
}
