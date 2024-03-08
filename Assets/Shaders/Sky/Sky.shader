Shader "PaintRenderer/Sky"
{
    HLSLINCLUDE
    #include "Sky.hlsl"
    ENDHLSL

    Properties
    {
        [MainTexture]
        _MainTexture ("Texture", 2D)  = "white" {}
    }

    SubShader
    {
        ZWrite Off
        ZTest Off
        Cull Off

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
