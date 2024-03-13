Shader "PaintRenderer/UISprite"
{
    HLSLINCLUDE
    #include "UISprite.hlsl"
    ENDHLSL

    Properties
    {
        [PerRendererData]
        _MainTex ("Sprite Texture", 2D) = "white" { }
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
