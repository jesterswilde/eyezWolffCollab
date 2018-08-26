Shader "Custom/Cel_shading" {
	Properties
	{
//		_MainTex("Texture", 2D) = "white" {}
		_Color("Main Color", Color) = (1, 1, 1, 1)
		_Threshold("Cel Threshold", Range (0.0, 20.0)) = 5
		_Ambient("Ambient Intensity", Range(0.0, 0.5)) = 0.1

		_OutlineWidth("Outline Width", Range(0.0, 0.5)) = 0.1
		_OutlineColor("Outline Color", Color) = (1, 0, 0, 1)
	}

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

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNormal : NORMAL;
			};

			float _Threshold;

			float LightToonShading( float3 norm, float3 lightDir)
			{
				float NdotL = max(0.0, dot(normalize(norm), normalize(lightDir)));
				return floor(NdotL * _Threshold) / (_Threshold - 0.5);
			}

			sampler2D _MainTex;
			float4 _MainTex_ST;

            v2f vert (appdata_full v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.worldNormal = mul(v.normal.xyz, (float3x3) unity_WorldToObject);
                return o;
            }

			fixed4 _LightColor0;
			half _Ambient;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb *= saturate(LightToonShading(i.worldNormal, _WorldSpaceLightPos0.xyz) + _Ambient) * _LightColor0.rgb;
                return col;
            }
			ENDCG
		}

}
