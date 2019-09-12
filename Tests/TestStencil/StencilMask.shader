Shader "Custom/StencilMask"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_StencilRef("StencilRef", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" }

		Pass
		{

			Stencil
			{
				Ref[_StencilRef]
				Comp Greater
				Pass Keep
			}
		}
    }

    FallBack off
}
