Shader "Custom/TextureAnimation"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Speed("Speed", float) = 5
		_UCount("UCount", float) = 1.0
		_VCount("VCount", float) = 1.0
		_Direction("Direction", float) = 1.0
    }
    SubShader
    {
		Tags { "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off

		Pass
		{
			CGPROGRAM

			#pragma multi_compile_instancing

			#pragma vertex vert
			#pragma fragment frag

			#include "Lighting.cginc"

			sampler2D _MainTex;
			float4    _MainTex_ST;

			UNITY_INSTANCING_BUFFER_START(Props)
				UNITY_DEFINE_INSTANCED_PROP(float, _Speed)
				UNITY_DEFINE_INSTANCED_PROP(float, _VCount)
				UNITY_DEFINE_INSTANCED_PROP(float, _UCount)
			UNITY_INSTANCING_BUFFER_END(Props)

			struct a2v
			{
				float4 vertex: POSITION;
				float4 texcoord: TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 pos: SV_POSITION;
				float3 uv: TEXCOORD0;
			};

			v2f vert(a2v v)
			{
				v2f o;

				UNITY_SETUP_INSTANCE_ID(v);

				o.pos = UnityObjectToClipPos(v.vertex);

				float vcount = UNITY_ACCESS_INSTANCED_PROP(Props, _VCount);
				float ucount = UNITY_ACCESS_INSTANCED_PROP(Props, _UCount);
				float speed = UNITY_ACCESS_INSTANCED_PROP(Props, _Speed);

				o.uv.xy = v.texcoord.xy * _MainTex_ST.xy * float2(ucount, vcount) + _MainTex_ST.zw;
				o.uv.y += _Time.y * speed;

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
