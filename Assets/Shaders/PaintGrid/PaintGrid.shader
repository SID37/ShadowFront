Shader "PaintRenderer/PaintGrid"
{
    HLSLINCLUDE
    #include "PaintGrid.hlsl"
    ENDHLSL

    Properties
    {
        [MainTexture]
        _MainTexture ("Texture", 2D)  = "black" {}

        _CutDistance ("Float", float)  = 0
    }

    SubShader
    {
        ZWrite Off
        ZTest  Off

        Pass
        {
            Name "PaintRendererPaintGrid"
            Tags { "LightMode" = "PaintRendererGridMode"}

            HLSLPROGRAM
                #pragma vertex Vert
                #pragma fragment Frag
            ENDHLSL
        }
    }
}