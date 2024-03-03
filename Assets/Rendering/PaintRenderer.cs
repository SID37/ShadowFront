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
    private GridRenderPass gridRenderPass;
    private SkyRenderPass skyRenderPass;

    private PaintRenderingContext paintContext;
    private bool enabled = false;

    public override void Create()
    {
        enabled = skyMaterial != null;
        if (!enabled) return;

        paintContext = new PaintRenderingContext();

        opaqueRenderPass = new OpaqueRenderPass(paintContext);
        opaqueRenderPass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;

        gridRenderPass = new GridRenderPass(paintContext);
        gridRenderPass.renderPassEvent = RenderPassEvent.BeforeRenderingSkybox;

        skyRenderPass = new SkyRenderPass(paintContext, skyMaterial);
        skyRenderPass.renderPassEvent = RenderPassEvent.AfterRenderingSkybox;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (!enabled) return;

        renderer.EnqueuePass(opaqueRenderPass);
        renderer.EnqueuePass(gridRenderPass);
        renderer.EnqueuePass(skyRenderPass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (!enabled) return;

        paintContext.Configure(renderingData);

        opaqueRenderPass.ConfigureInput(0);
        gridRenderPass.ConfigureInput(0);
        skyRenderPass.ConfigureInput(0);
    }

    protected override void Dispose(bool disposing)
    {
        if (skyRenderPass    != null) skyRenderPass.Dispose();
        if (gridRenderPass   != null) gridRenderPass.Dispose();
        if (opaqueRenderPass != null) opaqueRenderPass.Dispose();

        if (paintContext     != null) paintContext.Dispose();
    }
}
