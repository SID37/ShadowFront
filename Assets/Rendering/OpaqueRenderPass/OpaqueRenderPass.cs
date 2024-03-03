using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.Rendering.Universal;


class OpaqueRenderPass : ScriptableRenderPass
{
    private PaintRenderingContext paintContext;

    public OpaqueRenderPass(PaintRenderingContext paintContext)
    {
        this.paintContext = paintContext;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get("Opaque Renderer");
        CoreUtils.SetRenderTarget(cmd, paintContext.opaqueRenderTexture, paintContext.opaqueDepthTexture, ClearFlag.All);

        var camera = renderingData.cameraData.camera;
        camera.TryGetCullingParameters(out var cullingParameters);
        var cullingResults = context.Cull(ref cullingParameters);
        var renderListDesc = new RendererListDesc(new ShaderTagId("PaintRendererOpaqueMode"), cullingResults, camera);
        renderListDesc.renderQueueRange = RenderQueueRange.all;

        var renderList = context.CreateRendererList(renderListDesc);
        cmd.DrawRendererList(renderList);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public void Dispose()
    { }
}
