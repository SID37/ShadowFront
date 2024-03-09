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
        Pass
        {
            Name "PaintRendererOpaque"
            Tags { "LightMode" = "PaintRendererOpaqueMode"}
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest On

            HLSLPROGRAM
                #pragma vertex Vert
                #pragma fragment Frag
            ENDHLSL
        }
    }
}