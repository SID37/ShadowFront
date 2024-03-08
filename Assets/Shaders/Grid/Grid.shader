Shader "PaintRenderer/Grid"
{
    HLSLINCLUDE
    #include "Grid.hlsl"
    ENDHLSL

    Properties
    {
        _CutDistance ("Float", float)  = 0
    }

    SubShader
    {
        ZWrite Off
        ZTest  Off

        Pass
        {
            Name "PaintRendererGrid"
            Tags { "LightMode" = "PaintRendererGridMode"}

            HLSLPROGRAM
                #pragma vertex Vert
                #pragma fragment Frag
            ENDHLSL
        }
    }
}