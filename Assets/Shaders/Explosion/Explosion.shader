Shader "PaintRenderer/Explosion"
{
    HLSLINCLUDE
    #include "Explosion.hlsl"
    ENDHLSL

    Properties
    {
        [MainTexture]
        _MainTexture ("Texture", 2D)  = "white" {}
        _UVLayer  ("UV layer", Integer)  = 0
    }

    SubShader
    {
        Pass
        {
            Name "PaintRendererExplosion"
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