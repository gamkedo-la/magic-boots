Shader "Custom/KillerDistortionShader" 
{
	Properties 
	{
		_MainTex ("Main Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags 
		{
			"Queue"="Transparent"
		}
		
		Pass
		{
			ZWrite Off
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				half4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 screenuv : TEXCOORD1;
				half4 color : COLOR;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				o.screenuv = ((o.vertex.xy / o.vertex.w) + 1) * 0.5;
				o.color = v.color;
				return o;
			}

			//sampler2D _MainTex;
			uniform sampler2D _GlobalRefractionTex;

			float4 frag(v2f i) : SV_Target
			{
				float4 color = tex2D(_GlobalRefractionTex, i.uv) * i.color;
				float2 offset = tex2D(_OffsetTex, i.uv).xy * 2 - 1;

				float4 temp = tex2D(_GlobalRefractionTex, i.screenuv + offset);
				color.rgb = color.rgb * (1.0 - temp.a) + temp.rgb * temp.a;

				return color;
			}
			ENDCG
		}
	}
}
