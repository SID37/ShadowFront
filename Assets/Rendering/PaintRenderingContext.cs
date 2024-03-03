using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

class PaintRenderingContext: IDisposable
{
    public RTHandle tempCameraTexture;
    public RTHandle opaqueRenderTexture;
    public RTHandle opaqueDepthTexture;
    private RenderTextureDescriptor textureDescriptor;
    private RenderTextureDescriptor depthTextureDescriptor;

    public PaintRenderingContext()
    {
        textureDescriptor = new RenderTextureDescriptor(Screen.width, Screen.height, RenderTextureFormat.ARGBHalf, 0);
        depthTextureDescriptor = new RenderTextureDescriptor(Screen.width, Screen.height, RenderTextureFormat.RHalf, 16);
    }

    public void Configure(in RenderingData renderingData)
    {
        textureDescriptor.width = renderingData.cameraData.camera.pixelWidth;
        textureDescriptor.height = renderingData.cameraData.camera.pixelHeight;

        depthTextureDescriptor.width = renderingData.cameraData.camera.pixelWidth;
        depthTextureDescriptor.height = renderingData.cameraData.camera.pixelHeight;
        RenderingUtils.ReAllocateIfNeeded(ref tempCameraTexture,   textureDescriptor);
        RenderingUtils.ReAllocateIfNeeded(ref opaqueRenderTexture, textureDescriptor);
        RenderingUtils.ReAllocateIfNeeded(ref opaqueDepthTexture,  depthTextureDescriptor);
    }

    public void Dispose()
    {
        if (tempCameraTexture   != null) tempCameraTexture.Release();
        if (opaqueRenderTexture != null) opaqueRenderTexture.Release();
        if (opaqueDepthTexture  != null) opaqueDepthTexture.Release();
    }
}
