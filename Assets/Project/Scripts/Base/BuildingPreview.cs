using UnityEngine;

public class BuildingPreview : MonoBehaviour 
{
    [SerializeField] private MeshRenderer[] _meshRenderers;
    [SerializeField] private float _transparency;

    public void Initialize()
    {
        SetTransparency();
    }

    public void SetPreviewVisible()
    {
        foreach (MeshRenderer meshRenderer in _meshRenderers)
        {
            meshRenderer.enabled = true;
        }
    }

    public void SetPreviewInvisible()
    {
        foreach (MeshRenderer meshRenderer in _meshRenderers)
        {
            meshRenderer.enabled = false;
        }
    }

    private void SetTransparency()
    {
        const float SurfaceValue = 1;
        const int ZWriteValue = 0;
        const string SurfaceTitle = "_Surface";
        const string RenderTypeTitle = "RenderType";
        const string TransparentTitle = "Transparent";
        const string SrcBlendTitle = "_SrcBlend";
        const string DstBlendTitle = "_DstBlend";
        const string ZWriteTitle = "_ZWrite";
        const string ApplyAlphaTestTitle = "_ALPHATEST_ON";
        const string ApplyAlphaBlendTitle = "_ALPHABLEND_ON";
        const string ApplyAlphaPremultiplyTitle = "_ALPHAPREMULTIPLY_ON";

        foreach (MeshRenderer meshRenderer in _meshRenderers)
        {
            meshRenderer.material.SetFloat(SurfaceTitle, SurfaceValue);
            meshRenderer.material.SetOverrideTag(RenderTypeTitle, TransparentTitle);
            meshRenderer.material.SetInt(SrcBlendTitle, (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            meshRenderer.material.SetInt(DstBlendTitle, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            meshRenderer.material.SetInt(ZWriteTitle, ZWriteValue);
            meshRenderer.material.DisableKeyword(ApplyAlphaTestTitle);
            meshRenderer.material.EnableKeyword(ApplyAlphaBlendTitle);
            meshRenderer.material.DisableKeyword(ApplyAlphaPremultiplyTitle);
            meshRenderer.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            Material material = meshRenderer.material;
            Color color = material.color;
            color.a = _transparency;
            material.color = color;
        }
    }
}