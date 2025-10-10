using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ColorChanger : MonoBehaviour
{
    [SerializeField] private Material _opaqueMaterial;
    [SerializeField] private Material _transparentMaterial;

    private Renderer _renderer;
    private Material _currentMaterial;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        SetMaterialOpaque();
    }

    public void SetColor(Color color)
    {
        if (_renderer.material != null)
            _renderer.material.color = color;
    }

    public void SetRandomColor() =>
        SetColor(Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f));

    public void SetAlpha(float alpha)
    {
        if (_renderer.material != null)
        {
            var color = _renderer.material.color;
            color.a = Mathf.Clamp01(alpha);
            _renderer.material.color = color;
        }
    }

    public void SetMaterialTransparent()
    {
        if (_transparentMaterial != null)
            ApplyMaterial(_transparentMaterial);
    }

    public void SetMaterialOpaque()
    {
        if (_opaqueMaterial != null)
            ApplyMaterial(_opaqueMaterial);
    }

    private void ApplyMaterial(Material material)
    {
        if (_currentMaterial != material && _renderer != null)
        {
            _renderer.material = material;
            _currentMaterial = material;
        }
    }
}