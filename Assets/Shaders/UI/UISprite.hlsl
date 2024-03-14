#include "../Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Packing.hlsl"

CBUFFER_START(UnityPerMaterial)
    sampler2D _MainTex;
    float4 _RendererColor;
    float4 _Color;
CBUFFER_END


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

Varyings Vert(Attributes input)
{
    Varyings output;
    output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
    output.uv = input.uv;
    return output;
}

float4 Frag(Varyings input): SV_Target
{
    float4 customColor = tex2D(_MainTex, input.uv);
    return customColor * _RendererColor * _Color;
}
