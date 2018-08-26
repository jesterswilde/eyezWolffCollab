Shader "Custom/Cel_shadingFruit" 
{
	Properties
	{
//		_MainTex("Texture", 2D) = "white" {}
		_BrightColor("Bright Color", Color) = (1, 1, 1, 1)
		_DarkColor("Dark Color", Color) = (0, 0, 0, 1)
		_Threshold("Cel Threshold", int) = 5
		_Ambient("Ambient Intensity", Range(0.0, 0.5)) = 0.1

		_OutlineWidth("Outline Width", Range(0.0, 0.5)) = 0.1
		_OutlineColor("Outline Color", Color) = (1, 0, 0, 1)

		_MeltY("MeltY", Float) = 0
		_MeltXZ("MeltXZ", Float) = 0

		_IsCloud("Could", Range(0,1)) = 0
	}

	SubShader
	{
		Tags{"RenderType" = "Opaque" "LightMode" = "ForwardBase"}
		Pass
		{
			Stencil
			{
				Ref 4
				Comp always
				Pass replace
				ZFail keep
			}
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
			float _MeltY;
			float _MeltXZ;
			float _IsCloud;

			int _Threshold;

			float LightToonShading( float3 norm, float3 lightDir)
			{
				float NdotL = max(0.0, dot(normalize(norm), normalize(lightDir)));
				return floor(NdotL * _Threshold) / (_Threshold - 0.5);
			}

			float4 getNewVertexPosition(float4 objectSpaceVertex, float3 objectSpaceNormal)
			{
				float4 worldSpaceVertex = mul(unity_ObjectToWorld, objectSpaceVertex);
				float4 worldSpaceNormal = mul(unity_ObjectToWorld, float4(objectSpaceNormal, 0));

				float melt = worldSpaceVertex.y - _MeltY;
				//Kisses effect without saturate.
				if(_IsCloud == 0)
				{
					melt = 1 - saturate(melt);
				}
				else
				{
					melt = saturate(melt);
				}				

				worldSpaceVertex.xz += worldSpaceNormal.xz * melt * _MeltXZ;

//				if(worldSpaceVertex.y <= _MeltY)
//				{
//					worldSpaceVertex.y = _MeltY;
//				}
//
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
				float3 newNormal = cross(newTangent, newBitangent);
				o.normal = mul(newNormal.xyz, (float3x3) unity_WorldToObject);
				o.vertex = UnityObjectToClipPos(newVertexPos);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 _BrightColor;
			fixed4 _DarkColor;
			fixed4 _LightColor0;
			half _Ambient;

			float3 ColorMapping(fixed4 col, fixed4 brightCol, fixed4 darkCol)
			{
				float saturationVal = col.r;
				if(saturationVal == 1)
				{
					col = brightCol;
				}
				else if(saturationVal == 0)
				{
					col = darkCol;
				}
				else{
					col = lerp(darkCol, brightCol, saturationVal);
					}
					return col.rgb;
				}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
			    col.rgb *= saturate(LightToonShading(i.normal, _WorldSpaceLightPos0.xyz)) * _LightColor0;
			    col.rgb = ColorMapping(col, _BrightColor, _DarkColor);
				return col;
			}

			ENDCG
		}
//		//cel-shading pass.
//		Pass
//		{
//			Stencil
//			{
//				Ref 4
//				Comp always
//				Pass replace
//				ZFail keep
//			}
//			CGPROGRAM
//			#pragma vertex vert
//			#pragma fragment frag
//
//			#include "UnityCG.cginc"
//
//			struct v2f
//			{
//				float4 pos : SV_POSITION;
//				float2 uv : TEXCOORD0;
//				float3 worldNormal : NORMAL;
//			};
//
//			float _Threshold;
//
//			float LightToonShading( float3 norm, float3 lightDir)
//			{
//				float NdotL = max(0.0, dot(normalize(norm), normalize(lightDir)));
//				return floor(NdotL * _Threshold) / (_Threshold - 0.5);
//			}
//
//			sampler2D _MainTex;
//			float4 _MainTex_ST;
//
//            v2f vert (appdata_full v)
//            {
//                v2f o;
//                o.pos = UnityObjectToClipPos(v.vertex);
//                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
//                o.worldNormal = mul(v.normal.xyz, (float3x3) unity_WorldToObject);
//                return o;
//            }
//
//			fixed4 _LightColor0;
//			half _Ambient;
//
//            fixed4 frag (v2f i) : SV_Target
//            {
//                fixed4 col = tex2D(_MainTex, i.uv);
//                col.rgb *= saturate(LightToonShading(i.worldNormal, _WorldSpaceLightPos0.xyz) + _Ambient) * _LightColor0.rgb;
//                return col;
//            }
//			ENDCG
//		}
		//outline pass.
		Pass
		{

			Cull Off 
			ZWrite Off
			ZTest On

			Stencil
			{
				Ref 4
				Comp notequal
				Fail keep
				Pass replace

			}
			CGPROGRAM 
			#pragma vertex vert
			#pragma fragment frag

			struct vertInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};

			struct vertOutput
			{
				float4 pos : SV_POSITION;
				float3 normal : NORMAL;
				float4 color : COLOR;
			};

			float _MeltY;
			float _MeltXZ;
			float _IsCloud;
			float _OutlineWidth;
			fixed4 _OutlineColor;

			float4 getNewVertexPosition(float4 objectSpaceVertex, float3 objectSpaceNormal)
			{
				float4 worldSpaceVertex = mul(unity_ObjectToWorld, objectSpaceVertex);
				float4 worldSpaceNormal = mul(unity_ObjectToWorld, float4(objectSpaceNormal, 0));

				float melt = worldSpaceVertex.y - _MeltY;
				//Kisses effect without saturate.
				if(_IsCloud == 0)
				{
					melt = 1 - saturate(melt);
				}
				else
				{
					melt = saturate(melt);
				}
				worldSpaceVertex.xz += worldSpaceNormal.xz * melt * _MeltXZ;
//				if(worldSpaceVertex.y <= _MeltY)
//				{
//					worldSpaceVertex.y = _MeltY;
//				}
				return mul(unity_WorldToObject, worldSpaceVertex);
			}

			vertOutput vert(vertInput input)
			{
				vertOutput output;

				float4 newVertexPos = getNewVertexPosition(input.vertex, input.normal);
				float4 bitangent = float4(cross(input.normal, input.tangent), 0); 

				float vertOffset = 1;
				float4 UDir = getNewVertexPosition(input.vertex + input.tangent * vertOffset, input.normal);
				float4 VDir = getNewVertexPosition(input.vertex + bitangent * vertOffset, input.normal);

				float4 newTangent = UDir - newVertexPos;
				float4 newBitangent = VDir - newVertexPos;
				float3 newNormal = cross(newTangent, newBitangent);
				float4 newPos = newVertexPos;

				// normal extrusion technique
				float3 normal = normalize(newNormal);
				newPos += float4(normal, 0.0) * _OutlineWidth;

				// convert to world space
				output.pos = UnityObjectToClipPos(newPos);

				output.color = _OutlineColor;
				return output;
			}

			float4 frag(vertOutput IN) : COLOR
			{
				return IN.color;
			}

			ENDCG
		}
	}
}
