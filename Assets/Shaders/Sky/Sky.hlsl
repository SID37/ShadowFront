#include "../Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Packing.hlsl"
#include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

CBUFFER_START(UnityPerMaterial)
    TEXTURE2D(_MainTexture);
CBUFFER_END


float2 _TextureOffset;
float _ScreenScale;

float4 frag (Varyings input) : SV_Target
{
    float2 uv = float2(input.texcoord.x * _ScreenScale, input.texcoord.y) + _TextureOffset;
    float3 sky   = SAMPLE_TEXTURE2D(_MainTexture, sampler_LinearRepeat, uv).xyz;

    float3 result = sky;
    return float4(result, 1);
}
