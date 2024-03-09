#include "../Common.hlsl"


float4 Vert(float4 positionOS: POSITION): SV_POSITION
{
    return TransformObjectToHClip(positionOS.xyz);
}

void Frag(float4 positionCS: SV_POSITION)
{ }
