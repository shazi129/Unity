Shader "Custom/TextureAnimation"
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Speed("Speed", Float) = 5.0
		_UCount("UCount", Float) = 1.0
		_VCount("VCount", Float) = 1.0
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "Lighting.cginc"

			sampler2D _MainTex;
			float4    _MainTex_ST;
			float     _Speed;
			float     _UCount;
			float    _VCount;

			struct a2v
			{
				float4 vertex: POSITION;
				float4 texcoord: TEXCOORD0;
			};

			struct v2f
			{
				float4 pos: SV_POSITION;
				float4 uv: TEXCOORD0;
			};

			v2f vert(a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);


				o.uv.xy = v.texcoord.xy * _MainTex_ST.xy * float2(_UCount, _VCount)+ _MainTex_ST.zw;

				o.uv.x += _Time.y * _Speed;

				//o.uv.xy = frac(o.uv.xy);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				i.uv.xy = frac(i.uv.xy);
				fixed4 color = tex2D(_MainTex, i.uv.xy).rgba;
				return color;
			}

			ENDCG
		}
	}

	FallBack off
}
