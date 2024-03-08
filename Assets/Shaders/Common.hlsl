#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"


// Block Layout should be respected due to SRP Batcher
CBUFFER_START(UnityPerDraw)
// Space block Feature
float4x4 unity_ObjectToWorld;
float4x4 unity_WorldToObject;
float4 unity_LODFade; // x is the fade value ranging within [0,1]. y is x quantized into 16 levels
real4 unity_WorldTransformParams; // w is usually 1.0, or -1.0 for odd-negative scale transforms

// Render Layer block feature
// Only the first channel (x) contains valid data and the float must be reinterpreted using asuint() to extract the original 32 bits values.
float4 unity_RenderingLayer;

// Light Indices block feature
// These are set internally by the engine upon request by RendererConfiguration.
half4 unity_LightData;
half4 unity_LightIndices[2];

// Don't use half4 here as that makes shaders not SRP batcher compatible
float4 unity_ProbesOcclusion;

// Reflection Probe 0 block feature
// HDR environment map decode instructions
real4 unity_SpecCube0_HDR;
real4 unity_SpecCube1_HDR;

float4 unity_SpecCube0_BoxMax;          // w contains the blend distance
float4 unity_SpecCube0_BoxMin;          // w contains the lerp value
float4 unity_SpecCube0_ProbePosition;   // w is set to 1 for box projection
float4 unity_SpecCube1_BoxMax;          // w contains the blend distance
float4 unity_SpecCube1_BoxMin;          // w contains the sign of (SpecCube0.importance - SpecCube1.importance)
float4 unity_SpecCube1_ProbePosition;   // w is set to 1 for box projection

// Lightmap block feature
float4 unity_LightmapST;
float4 unity_DynamicLightmapST;

// SH block feature
real4 unity_SHAr;
real4 unity_SHAg;
real4 unity_SHAb;
real4 unity_SHBr;
real4 unity_SHBg;
real4 unity_SHBb;
real4 unity_SHC;

// Velocity
float4x4 unity_MatrixPreviousM;
float4x4 unity_MatrixPreviousMI;
//X : Use last frame positions (right now skinned meshes are the only objects that use this
//Y : Force No Motion
//Z : Z bias value
//W : Camera only
float4 unity_MotionVectorsParams;
CBUFFER_END

float4x4 glstate_matrix_projection;
float4x4 unity_MatrixV;
float4x4 unity_MatrixInvV;
float4x4 unity_MatrixInvP;
float4x4 unity_MatrixVP;
float4x4 unity_MatrixInvVP;
float4 unity_StereoScaleOffset;
float4x4 unity_prev_MatrixM;
float4x4 unity_prev_MatrixIM;

#define UNITY_MATRIX_M unity_ObjectToWorld
#define UNITY_MATRIX_I_M unity_WorldToObject
#define UNITY_MATRIX_V unity_MatrixV
#define UNITY_MATRIX_I_V unity_MatrixInvV
#define UNITY_MATRIX_VP unity_MatrixVP
#define UNITY_PREV_MATRIX_M unity_prev_MatrixM
#define UNITY_PREV_MATRIX_I_M unity_prev_MatrixIM
#define UNITY_MATRIX_P glstate_matrix_projection


#define SLICE_ARRAY_INDEX 0

#define TEXTURE2D_X(textureName)                                        TEXTURE2D(textureName)
#define TEXTURE2D_X_PARAM(textureName, samplerName)                     TEXTURE2D_PARAM(textureName, samplerName)
#define TEXTURE2D_X_ARGS(textureName, samplerName)                      TEXTURE2D_ARGS(textureName, samplerName)
#define TEXTURE2D_X_HALF(textureName)                                   TEXTURE2D_HALF(textureName)
#define TEXTURE2D_X_FLOAT(textureName)                                  TEXTURE2D_FLOAT(textureName)

#define LOAD_TEXTURE2D_X(textureName, unCoord2)                         LOAD_TEXTURE2D(textureName, unCoord2)
#define LOAD_TEXTURE2D_X_LOD(textureName, unCoord2, lod)                LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)
#define SAMPLE_TEXTURE2D_X(textureName, samplerName, coord2)            SAMPLE_TEXTURE2D(textureName, samplerName, coord2)
#define SAMPLE_TEXTURE2D_X_LOD(textureName, samplerName, coord2, lod)   SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)
#define GATHER_TEXTURE2D_X(textureName, samplerName, coord2)            GATHER_TEXTURE2D(textureName, samplerName, coord2)
#define GATHER_RED_TEXTURE2D_X(textureName, samplerName, coord2)        GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)
#define GATHER_GREEN_TEXTURE2D_X(textureName, samplerName, coord2)      GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)
#define GATHER_BLUE_TEXTURE2D_X(textureName, samplerName, coord2)       GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)



#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GlobalSamplers.hlsl"
