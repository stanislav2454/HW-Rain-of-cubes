using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ColorChanger : MonoBehaviour
{
    [SerializeField] private Material _material;
    private Renderer _renderer;

    private void Awake()
    {
        CacheComponents();
    }

    private void OnDestroy()
    {
        if (_material != null)
            DestroyImmediate(_material);
    }

    public void SetColor(Color color)
    {
        if (_material != null)
            _material.color = color;
    }

    public void SetRandomColor()
    {
        if (_material != null)
            _material.color = Random.ColorHSV();
    }

    private void CacheComponents()
    {
        if (TryGetComponent(out _renderer) == false)
        {
            Debug.LogError("Renderer component not found!", this);
            return;
        }

        _material = _renderer.material;
    }
}