using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;

public class PaintPipelineInstance : RenderPipeline
{
    Material skyMaterial = null;

    public PaintPipelineInstance (Material skyMaterial) {
        this.skyMaterial = skyMaterial;
    }

    protected override void Render(ScriptableRenderContext context, Camera[] cameras) {
        foreach (Camera camera in cameras)
        {
            var cmd = new CommandBuffer() { name = "Paint Rendering" };

            camera.TryGetCullingParameters(out var cullingParameters);
            var cullingResults = context.Cull(ref cullingParameters);
            context.SetupCameraProperties(camera);

            {
                var sampleName = "Clear render target";
                cmd.BeginSample(sampleName);
                cmd.ClearRenderTarget(true, true, Color.yellow);
                cmd.EndSample(sampleName);
            }

            {
                var sampleName = "Draw sky background";
                cmd.BeginSample(sampleName);

                float hFieldOfView = 60; // camera.fieldOfView
                float wFieldOfView = 90; // ~= 90 for 16/9
                float screenScale  = camera.pixelWidth / (float) camera.pixelHeight;
                var angles = camera.transform.eulerAngles;
                skyMaterial.SetVector(Shader.PropertyToID("_TextureOffset"), new Vector2(angles.y / wFieldOfView, -angles.x / hFieldOfView));
                skyMaterial.SetFloat(Shader.PropertyToID("_ScreenScale"), screenScale);
                Blitter.BlitTexture(cmd, new Vector4(1, 1, 0, 0), skyMaterial, 0);

                cmd.EndSample(sampleName);
            }

            {
                var sampleName = "Draw opaque models";
                cmd.BeginSample(sampleName);

                ShaderTagId shaderTagId = new ShaderTagId("PaintRendererOpaqueMode");
                var renderListDesc = new RendererListDesc(shaderTagId, cullingResults, camera);
                renderListDesc.renderQueueRange = RenderQueueRange.all;

                var renderList = context.CreateRendererList(renderListDesc);
                cmd.DrawRendererList(renderList);

                cmd.EndSample(sampleName);
            }

            {
                var sampleName = "Draw grid meshes";
                cmd.BeginSample(sampleName);

                ShaderTagId shaderTagId = new ShaderTagId("PaintRendererGridMode");
                var renderListDesc = new RendererListDesc(shaderTagId, cullingResults, camera);
                renderListDesc.renderQueueRange = RenderQueueRange.all;

                var renderList = context.CreateRendererList(renderListDesc);
                cmd.DrawRendererList(renderList);

                cmd.EndSample(sampleName);
            }

            context.ExecuteCommandBuffer(cmd);
            cmd.Release();
            context.Submit();
        }
    }
}