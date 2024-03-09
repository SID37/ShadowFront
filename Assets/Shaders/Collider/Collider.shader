Shader "PaintRenderer/Collider"
{
    HLSLINCLUDE
    #include "Collider.hlsl"
    ENDHLSL

    SubShader
    {
        Pass
        {
            Name "PaintRendererColldier"
            Tags { "LightMode" = "PaintRendererColliderMode"}
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            ZTest On

            HLSLPROGRAM
                #pragma vertex Vert
                #pragma fragment Frag
            ENDHLSL
        }
    }
}