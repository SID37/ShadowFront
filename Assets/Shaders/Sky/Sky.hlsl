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
    float4 blit = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, input.texcoord);
    float2 uv = input.texcoord;
    float3 sky = tex2D(_MainTexture, float2(uv.x * _ScreenScale, uv.y) + _TextureOffset);
    return float4(sky * (1.0 - blit.w) + blit.xyz * blit.w, 1);
}