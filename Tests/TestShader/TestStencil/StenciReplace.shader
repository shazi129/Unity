Shader "Custom/StencilReplace"
{
    Properties
    {
		_StencilRef("StencilRef", Float) = 10
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" }

		Pass
		{
			Stencil
			{
				Ref[_StencilRef]
				Comp Always
				Pass Replace
			}
		}
    }
}
