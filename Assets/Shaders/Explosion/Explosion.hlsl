#include "../Common.hlsl"

struct Attributes
{
    float4 positionOS: POSITION;
    float2 uv0:        TEXCOORD0;
    float2 uv1:        TEXCOORD1;
    float2 uv2:        TEXCOORD2;
    float2 uv3:        TEXCOORD3;
};

struct Varyings
{
    float4 positionCS: SV_POSITION;
    float2 uv:         TEXCOORD0;
};

CBUFFER_START(UnityPerMaterial)
    sampler2D _MainTexture;
    int _UVLayer;
CBUFFER_END


Varyings Vert(Attributes input)
{
    Varyings output;
    output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
         if (_UVLayer == 0) output.uv = input.uv0;
    else if (_UVLayer == 1) output.uv = input.uv1;
    else if (_UVLayer == 2) output.uv = input.uv2;
    else if (_UVLayer == 3) output.uv = input.uv3;
    return output;
}

float4 Frag(Varyings input): SV_Target
{
    float4 customColor = tex2D(_MainTexture, input.uv);
    return customColor;
}
