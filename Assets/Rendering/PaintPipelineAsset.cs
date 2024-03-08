using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/PaintPipelineAsset")]
public class PaintPipelineAsset: RenderPipelineAsset
{
    public Material skyShader;

    protected override RenderPipeline CreatePipeline() {
        return new PaintPipelineInstance(skyShader);
    }
}