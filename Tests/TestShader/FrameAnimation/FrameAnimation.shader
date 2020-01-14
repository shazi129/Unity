Shader "Custom/FrameAnimation"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
		_FrameTex("Frame Texture", 2D) = "white"{}

		_Speed("Speed", float) = 30
		_FrameCount("FrameCount", float) = 16

		_MainTexSize("Main Textrue Size", float) = 1024
		_FrameTexSize("Frame Textrue size", float) = 4
    }
    SubShader
    {
		Tags { "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off

		Pass
		{
			CGPROGRAM

		    #pragma enable_d3d11_debug_symbols

			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			sampler2D _FrameTex;

			float4    _MainTex_ST;

			float      _FrameCount;
			float     _Speed;
			float      _MainTexSize;
			float      _FrameTexSize;

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
				o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				
				float frameIndex = floor((_Time.y * _Speed) % _FrameCount);
				float row = floor(frameIndex / _FrameTexSize);
				float col = floor(frameIndex % _FrameTexSize);
				o.uv.z = frac(row / _FrameTexSize) + 0.001;
				o.uv.w = frac(col / _FrameTexSize) + 0.001;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 frame_data = tex2D(_FrameTex, i.uv.zw).rgba;

				fixed2 frame = frame_data.xy + frame_data.zw * i.uv.xy;
				fixed4 color = tex2D(_MainTex, frame).rgba;
				return color;
			}

			ENDCG
		}
    }
    FallBack off
}
