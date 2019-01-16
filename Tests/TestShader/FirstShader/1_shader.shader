
Shader "Custom/1_shader" {

	Properties{
		_Color("Color Tint", Color) = (1.0, 1.0, 1.0, 1.0)
		_Offset("Offset", Vector) = (0, 0, 0, 0)
	}

	SubShader{
		Pass {
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			fixed4 _Color;
			fixed4 _Offset;

			float4 vert(float4 v : POSITION) : SV_POSITION {
				//return mul(UNITY_MATRIX_MVP, v);
				return UnityObjectToClipPos(v) + _Offset;
			}

			fixed4 frag() : SV_Target {
				return _Color;
			}

			ENDCG
		}
	}
}
