using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.Rendering.Universal;


class GridRenderPass : ScriptableRenderPass
{
    private PaintRenderingContext paintContext;

    public GridRenderPass(PaintRenderingContext paintContext)
    {
        this.paintContext = paintContext;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get("Grid Renderer");
        CoreUtils.SetRenderTarget(cmd, paintContext.gridRenderTexture, paintContext.gridDepthTexture, ClearFlag.All);

        Matrix4x4 viewProjectionMatrix = renderingData.cameraData.GetGPUProjectionMatrix() * renderingData.cameraData.GetViewMatrix();
        cmd.SetGlobalMatrix(Shader.PropertyToID("_InvertViewProjectionMatrix"), viewProjectionMatrix.inverse);
        cmd.SetGlobalTexture(Shader.PropertyToID("_CameraDepthTexture"), paintContext.opaqueDepthTexture);

        var camera = renderingData.cameraData.camera;
        camera.TryGetCullingParameters(out var cullingParameters);
        var cullingResults = context.Cull(ref cullingParameters);
        var renderListDesc = new RendererListDesc(new ShaderTagId("PaintRendererGridMode"), cullingResults, camera);
        renderListDesc.renderQueueRange = RenderQueueRange.all;

        var renderList = context.CreateRendererList(renderListDesc);
        cmd.DrawRendererList(renderList);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public void Dispose()
    { }
}
