Shader "PaintRenderer/Opaque"
{
    HLSLINCLUDE
    #include "Opaque.hlsl"
    ENDHLSL

    Properties
    {
        [MainTexture] _MainTexture ("Texture", 2D)  = "white" {}
    }

    SubShader
    {
        ZWrite On

        Pass
        {
            Name "PaintRendererOpaque"
            Tags { "LightMode" = "PaintRendererOpaqueMode"}

            HLSLPROGRAM
                #pragma vertex Vert
                #pragma fragment Frag
            ENDHLSL
        }
    }
}