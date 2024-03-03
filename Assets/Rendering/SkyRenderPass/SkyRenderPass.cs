using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.Rendering.Universal;


class SkyRenderPass : ScriptableRenderPass
{
    private Material material;
    private PaintRenderingContext paintContext;

    public SkyRenderPass(PaintRenderingContext paintContext, Material material)
    {
        this.paintContext = paintContext;
        this.material = material;
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    { }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        Camera camera = renderingData.cameraData.camera;
        float hFieldOfView = 60; // camera.fieldOfView
        float wFieldOfView = 90; // ~= 90 for 16/9
        float screenScale  = camera.pixelWidth / (float) camera.pixelHeight;
        var angles = camera.transform.eulerAngles;
        material.SetVector(Shader.PropertyToID("_TextureOffset"), new Vector2(angles.y / wFieldOfView, -angles.x / hFieldOfView));
        material.SetFloat(Shader.PropertyToID("_ScreenScale"), screenScale);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        RTHandle cameraTargetHandle = renderingData.cameraData.renderer.cameraColorTargetHandle;
        // TODO: fix
        if (cameraTargetHandle.rt == null)
            return;

        Vector2 viewportScale = cameraTargetHandle.useScaling ? new Vector2(cameraTargetHandle.rtHandleProperties.rtHandleScale.x, cameraTargetHandle.rtHandleProperties.rtHandleScale.y) : Vector2.one;

        CommandBuffer cmd = CommandBufferPool.Get("Sky Renderer");

        CoreUtils.SetRenderTarget(cmd, cameraTargetHandle, paintContext.opaqueDepthTexture);
        Blitter.BlitTexture(cmd, paintContext.opaqueRenderTexture, viewportScale, material, 0);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public void Dispose()
    { }
}
