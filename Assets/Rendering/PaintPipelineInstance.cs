using UnityEngine;
using UnityEngine.Rendering;

public class PaintPipelineInstance : RenderPipeline
{
    Material skyMaterial;

    public PaintPipelineInstance (Material skyMaterial) {
        this.skyMaterial = skyMaterial;
    }

    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (Camera camera in cameras)
        {
            using (var paint = new PaintRenderContext(context, camera))
            {
                using (paint.NewSample("Clear render target"))
                    paint.cmd.ClearRenderTarget(true, true, Color.yellow);

                using (paint.NewSample("Draw sky background"))
                {
                    float hFieldOfView = 60; // camera.fieldOfView
                    float wFieldOfView = 90; // ~= 90 for 16/9
                    float screenScale  = camera.pixelWidth / (float) camera.pixelHeight;
                    var angles = camera.transform.eulerAngles;
                    skyMaterial.SetVector(Shader.PropertyToID("_TextureOffset"), new Vector2(angles.y / wFieldOfView, -angles.x / hFieldOfView));
                    skyMaterial.SetFloat(Shader.PropertyToID("_ScreenScale"), screenScale);
                    paint.Blit(skyMaterial);
                }

                using (paint.NewSample("Draw depth colliders"))
                    paint.DrawByShaderTag(new ShaderTagId("PaintRendererColliderMode"));

                using (paint.NewSample("Draw opaque models"))
                    paint.DrawByShaderTag(new ShaderTagId("PaintRendererOpaqueMode"));
            }
            context.Submit();
        }
    }
}
