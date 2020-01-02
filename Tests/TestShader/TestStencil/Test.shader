// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Test"
{
	Properties
	{
		_GreyFactor("Greyd Factor", Float) = 1
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 10
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15
	}

	SubShader
	{
		Tags { "RenderType" = "Transparent" }

		Pass
		{
		}
	}
}
