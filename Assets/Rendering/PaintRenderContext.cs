using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;

class PaintRenderContext: IDisposable
{
    public ScriptableRenderContext context;
    public CommandBuffer cmd;
    public Camera camera;
    public CullingResults cullingResults;

    public struct Sample : IDisposable
    {
        PaintRenderContext context;
        string sampleName;

        public Sample(PaintRenderContext context, string sampleName)
        {
            this.context = context;
            this.sampleName = sampleName;
            context.cmd.BeginSample(sampleName);
        }

        public void Dispose()
        {
            context.cmd.EndSample(sampleName);
        }
    }

    public PaintRenderContext(ScriptableRenderContext context, Camera camera)
    {
        this.camera = camera;
        this.context = context;
        cmd = new CommandBuffer();
        cmd.name = "Paint Rendering";

        camera.TryGetCullingParameters(out var cullingParameters);
        cullingResults = context.Cull(ref cullingParameters);
        context.SetupCameraProperties(camera);
    }

    public void Blit(Material material, int pass = 0)
    {
        Blitter.BlitTexture(cmd, new Vector4(1, 1, 0, 0), material, pass);
    }

    public void DrawByShaderTag(ShaderTagId shaderTagId)
    {
        var renderListDesc = new RendererListDesc(shaderTagId, cullingResults, camera);
        renderListDesc.renderQueueRange = RenderQueueRange.all;

        var renderList = context.CreateRendererList(renderListDesc);
        cmd.DrawRendererList(renderList);
    }

    public Sample NewSample(string name)
    {
        return new Sample(this, name);
    }

    public void Dispose()
    {
        context.ExecuteCommandBuffer(cmd);
        cmd.Release();
    }
}
