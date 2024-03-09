#include "../Common.hlsl"

TEXTURE2D(_CameraDepthTexture);
float4x4 _InvertViewProjectionMatrix;

CBUFFER_START(UnityPerMaterial)
    TEXTURE2D(_MainTexture);
    float _CutDistance;
CBUFFER_END

struct Attributes
{
    float4 positionOS: POSITION;
    float2 uv:         TEXCOORD0;
    float3 normalOS:   NORMAL;
};

struct Varyings
{
    float4 positionCS: SV_POSITION;
    float2 uv:         TEXCOORD0;
    float3 normalVS:   NORMAL;
    float3 uvCS:       TEXCOORD1;
};

Varyings Vert(Attributes input)
{
    Varyings output;
    output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
    output.normalVS = TransformWorldToViewNormal(TransformObjectToWorldNormal(input.normalOS, false), false);
    output.uvCS = output.positionCS.xyz / output.positionCS.w;
    output.uv = input.uv;
    return output;
}

void Frag(Varyings input)
{}

// float4 Frag(Varyings input): SV_Target
// {
//     float3 uv = input.uvCS;
//     uv = ComputeNormalizedDeviceCoordinatesWithZ(uv);
//     // float depth = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_PointClamp, uv.xy).r;
//     float4 value = SAMPLE_TEXTURE2D(_MainTexture, sampler_LinearClamp, input.uv);
//     if (value.w == 0) discard;

//     float k = 0;

//     // if (depth > uv.z)
//     // {
//     //     float3 powWS = ComputeWorldSpacePosition(uv.xy, uv.z,  _InvertViewProjectionMatrix);
//     //     float3 depWS = ComputeWorldSpacePosition(uv.xy, depth, _InvertViewProjectionMatrix);
//     //     float3 delta = powWS - depWS;
//     //     float d2 = dot(delta, delta);
//     //     float c2 = _CutDistance * _CutDistance;
//     //     if (depth > uv.z && d2 > c2)
//     //         discard;
//     //     k = d2 / c2;
//     // }

//     return float4(value.xyz, value.w * clamp(1 - k*k*k*k, 0, 1));
// }
