#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

CBUFFER_START(UnityPerMaterial)
    float4 _BaseColor;
    TEXTURE2D(_MainTexture);
    TEXTURE2D(_NoiseTexture);
CBUFFER_END

TEXTURE2D(_CameraDepthTexture);
TEXTURE2D(_GridColorTexture);

float2 _TextureOffset;
float _ScreenScale;

float4 frag (Varyings input) : SV_Target
{
    float4 opaque = SAMPLE_TEXTURE2D(_BlitTexture,        sampler_PointClamp,  input.texcoord);
    float4 grid   = SAMPLE_TEXTURE2D(_GridColorTexture,   sampler_PointClamp,  input.texcoord);
    float3 noise  = SAMPLE_TEXTURE2D(_NoiseTexture,       sampler_PointRepeat, input.texcoord).xyz;

    float2 uv = float2(input.texcoord.x * _ScreenScale, input.texcoord.y) + _TextureOffset;
    float3 sky   = SAMPLE_TEXTURE2D(_MainTexture, sampler_LinearRepeat, uv).xyz;

    grid.w *= noise;

    float3 result = sky;
    result = result * (1.0 - grid.w)   + grid.xyz  * grid.w;
    result = result * (1.0 - opaque.w) + opaque.xyz * opaque.w * sky;
    return float4(result, 1);
}
