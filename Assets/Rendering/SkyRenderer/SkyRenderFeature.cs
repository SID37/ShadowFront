using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.Rendering.Universal;

public class SkyRenderFeature : ScriptableRendererFeature
{
    class SkyRenderPass : ScriptableRenderPass
    {
        private RenderTextureDescriptor textureDescriptor;
        private RTHandle tempTexture;
        private Material material;

        public SkyRenderPass(Material material)
        {
            this.material = material;
            textureDescriptor = new RenderTextureDescriptor(Screen.width, Screen.height, RenderTextureFormat.ARGBHalf, 0);
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            textureDescriptor.width = cameraTextureDescriptor.width;
            textureDescriptor.height = cameraTextureDescriptor.height;
            RenderingUtils.ReAllocateIfNeeded(ref tempTexture, textureDescriptor);
        }

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

            CommandBuffer cmd = CommandBufferPool.Get("Sky Renderer");

            Blit(cmd, cameraTargetHandle, tempTexture, material, 0);
            Blit(cmd, tempTexture, cameraTargetHandle, material, 0);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public void Dispose()
        { }
    }

    public Material material;
    private SkyRenderPass skyRenderPass;

    public override void Create()
    {
        skyRenderPass = new SkyRenderPass(material);
        skyRenderPass.renderPassEvent = RenderPassEvent.AfterRenderingSkybox;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(skyRenderPass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        skyRenderPass.ConfigureInput(0);
    }

    protected override void Dispose(bool disposing)
    {
        if (skyRenderPass != null) skyRenderPass.Dispose();
    }
}
