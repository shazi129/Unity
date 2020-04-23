Shader "Custom/FrameAnimation"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
		_Speed("Speed", float) = 30
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
			float4    _MainTex_ST;

			float     _AnimationSize;         //动画的大小，[0-1]
			uniform float4 _FrameData[100];  //帧数据
			uniform float _FrameCount;        //一共多少帧
			float     _Speed;

			struct a2v
			{
				float4 vertex: POSITION;
				float4 texcoord: TEXCOORD0;
			};

			struct v2f
			{
				float4 pos: SV_POSITION;
				float3 uv: TEXCOORD0;
			};

			v2f vert(a2v v)
			{
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float frameIndex = floor((_Time.y * _Speed) % _FrameCount);

				//当前帧数据
				fixed4 curFrame = _FrameData[frameIndex];

				fixed2 frame = curFrame.xy + curFrame.zw * i.uv.xy;
				fixed4 color = tex2D(_MainTex, frame).rgba;
				return color;
			}

			ENDCG
		}
    }
    FallBack off
}
