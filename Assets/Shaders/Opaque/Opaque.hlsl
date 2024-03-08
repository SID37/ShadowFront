#include "../Common.hlsl"

struct Attributes
{
    float4 positionOS: POSITION;
    float2 uv:         TEXCOORD0;
};

struct Varyings
{
    float4 positionCS: SV_POSITION;
    float2 uv:         TEXCOORD0;
};

CBUFFER_START(UnityPerMaterial)
    sampler2D _MainTexture;
CBUFFER_END


Varyings Vert(Attributes input)
{
    Varyings output;
    output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
    output.uv = input.uv;
    return output;
}

float4 Frag(Varyings input): SV_Target
{
    float4 customColor = tex2D(_MainTexture, input.uv);
    return customColor;
}
