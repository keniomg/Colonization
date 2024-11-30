using UnityEngine;

public class BuildingPreview : MonoBehaviour 
{
    [SerializeField] private MeshRenderer[] _meshRenderers;
    [SerializeField] private float _transparency;

    private void Awake()
    {
        SetTransparency();
    }

    public void SetPreviewVisibility(bool isVisible)
    {
        foreach (MeshRenderer meshRenderer in _meshRenderers)
        {
            meshRenderer.enabled = isVisible;
        }
    }

    private void SetTransparency()
    {
        foreach (MeshRenderer meshRenderer in _meshRenderers)
        {
            meshRenderer.material.SetFloat("_Surface", 1);
            meshRenderer.material.SetOverrideTag("RenderType", "Transparent");
            meshRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            meshRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            meshRenderer.material.SetInt("_ZWrite", 0);
            meshRenderer.material.DisableKeyword("_ALPHATEST_ON");
            meshRenderer.material.EnableKeyword("_ALPHABLEND_ON");
            meshRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            meshRenderer.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            Material material = meshRenderer.material;
            Color color = material.color;
            color.a = _transparency;
            material.color = color;
        }
    }
}