Shader "Unlit/Skybox"
{
    Properties
    {
        [MainColor]   _BaseColor   ("Color", Color) = (1,1,1,1)
        [MainTexture] _MainTexture ("Texture", 2D)  = "white" {}
    }
    SubShader
    {
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                sampler2D _MainTexture;
            CBUFFER_END

            sampler2D _CameraDepthTexture;

            float2 _TextureOffset;
            float _ScreenScale;

            float4 frag (Varyings input) : SV_Target
            {
                float depth = tex2D(_CameraDepthTexture, input.texcoord);
                if (depth > 0) discard;
                float2 uv = input.texcoord;
                return tex2D(_MainTexture, float2(uv.x * _ScreenScale, uv.y) + _TextureOffset);
            }
            ENDHLSL
        }
    }
}
