Shader "PaintRenderer/UISprite"
{
    HLSLINCLUDE
    #include "UISprite.hlsl"
    ENDHLSL

    Properties
    {
        [PerRendererData]
        _MainTex ("Sprite Texture", 2D) = "white" { }
        _Color ("Tint", Color) = (1.000000,1.000000,1.000000,1.000000)
        [HideInInspector]
        _RendererColor ("RendererColor", Color) = (1.000000,1.000000,1.000000,1.000000)
    }

    SubShader
    {
        Pass
        {
            Name "PaintUISprite"
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest Off
            Cull Off

            HLSLPROGRAM
                #pragma vertex Vert
                #pragma fragment Frag
            ENDHLSL
        }
    }
}
