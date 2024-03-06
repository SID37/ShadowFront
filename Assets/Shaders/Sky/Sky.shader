Shader "PaintRenderer/Sky"
{
    HLSLINCLUDE
    #include "Sky.hlsl"
    ENDHLSL

    Properties
    {
        [MainColor]
        _BaseColor   ("Color", Color) = (1,1,1,1)
        [MainTexture]
        _MainTexture ("Texture", 2D)  = "white" {}

        _NoiseTexture ("Texture", 2D)  = "white" {}
    }

    SubShader
    {
        ZWrite Off

        Pass
        {
            Name "PaintSkyRenderer"

            HLSLPROGRAM
                #pragma vertex Vert
                #pragma fragment frag
            ENDHLSL
        }
    }
}
