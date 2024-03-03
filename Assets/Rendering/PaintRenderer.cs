using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.Rendering.Universal;


public class PainRenderer : ScriptableRendererFeature
{
    public Material skyMaterial;

    private OpaqueRenderPass opaqueRenderPass;
    private SkyRenderPass skyRenderPass;

    private PaintRenderingContext paintContext;
    private bool enabled = false;

    public override void Create()
    {
        enabled = skyMaterial != null;
        if (!enabled) return;

        paintContext = new PaintRenderingContext();

        skyRenderPass = new SkyRenderPass(paintContext, skyMaterial);
        skyRenderPass.renderPassEvent = RenderPassEvent.AfterRenderingSkybox;

        opaqueRenderPass = new OpaqueRenderPass(paintContext);
        opaqueRenderPass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (!enabled) return;

        renderer.EnqueuePass(skyRenderPass);
        renderer.EnqueuePass(opaqueRenderPass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (!enabled) return;

        paintContext.Configure(renderingData);
        skyRenderPass.ConfigureInput(0);
        opaqueRenderPass.ConfigureInput(0);
    }

    protected override void Dispose(bool disposing)
    {
        if (opaqueRenderPass != null) opaqueRenderPass.Dispose();
        if (skyRenderPass    != null) skyRenderPass.Dispose();
        if (paintContext     != null) paintContext.Dispose();
    }
}
