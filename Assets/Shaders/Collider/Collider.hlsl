#include "../Common.hlsl"

float4 Vert(float4 positionOS: POSITION): SV_POSITION
{
    return TransformObjectToHClip(positionOS.xyz);
}

float4 Frag(float4 positionCS: SV_POSITION): SV_Target
{
    return float4(0, 0, 0, 0);
}
